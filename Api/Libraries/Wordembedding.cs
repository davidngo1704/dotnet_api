using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;



// ============================================================
// PHẦN 1: Cấu trúc cơ bản — Vector cho mỗi token
// ============================================================


/// <summary>
/// Mỗi token/từ được biểu diễn bằng 1 mảng float nhiều chiều (embedding vector).
/// Ví dụ: "cat" → [0.23f, -0.11f, 0.87f, ..., 0.05f]  (768 chiều)
/// </summary>
/// 


public class EmbeddingVector
{
    public string Token { get; }
    public float[] Values { get; }
    public int Dimensions => Values.Length;

    public EmbeddingVector(string token, float[] values)
    {
        Token = token;
        Values = values;
    }

    // ── Độ tương đồng cosine (1 = giống hệt, 0 = vuông góc, -1 = ngược) ──
    public float CosineSimilarity(EmbeddingVector other)
    {
        if (Dimensions != other.Dimensions)
            throw new ArgumentException("Vectors phải cùng số chiều");

        float dot = 0f, magA = 0f, magB = 0f;
        for (int i = 0; i < Dimensions; i++)
        {
            dot += Values[i] * other.Values[i];
            magA += Values[i] * Values[i];
            magB += other.Values[i] * other.Values[i];
        }
        return dot / (MathF.Sqrt(magA) * MathF.Sqrt(magB) + 1e-8f);
    }

    // ── Cộng hai vector: king + woman ──
    public static EmbeddingVector operator +(EmbeddingVector a, EmbeddingVector b)
    {
        var result = new float[a.Dimensions];
        for (int i = 0; i < a.Dimensions; i++) result[i] = a.Values[i] + b.Values[i];
        return new EmbeddingVector($"({a.Token}+{b.Token})", result);
    }

    // ── Trừ hai vector: king - man ──
    public static EmbeddingVector operator -(EmbeddingVector a, EmbeddingVector b)
    {
        var result = new float[a.Dimensions];
        for (int i = 0; i < a.Dimensions; i++) result[i] = a.Values[i] - b.Values[i];
        return new EmbeddingVector($"({a.Token}-{b.Token})", result);
    }

    // ── Khoảng cách Euclidean ──
    public float EuclideanDistance(EmbeddingVector other)
    {
        float sum = 0f;
        for (int i = 0; i < Dimensions; i++)
        {
            float diff = Values[i] - other.Values[i];
            sum += diff * diff;
        }
        return MathF.Sqrt(sum);
    }

    // ── Normalize về unit vector ──
    public EmbeddingVector Normalize()
    {
        float mag = MathF.Sqrt(Values.Sum(v => v * v));
        return new EmbeddingVector(Token, Values.Select(v => v / (mag + 1e-8f)).ToArray());
    }

    public override string ToString()
        => $"[{string.Join(", ", Values.Take(4).Select(v => v.ToString("F3")))}... ({Dimensions}D)]";
}


// ============================================================
// PHẦN 2: Kho lưu trữ embedding (Embedding Store)
// ============================================================

public class EmbeddingStore
{
    private readonly Dictionary<string, EmbeddingVector> _store = new();

    public void Add(EmbeddingVector embedding) => _store[embedding.Token] = embedding;

    public EmbeddingVector? Get(string token)
        => _store.TryGetValue(token, out var v) ? v : null;

    /// <summary>
    /// Tìm top-K từ gần nhất với vector đầu vào (dùng cosine similarity)
    /// </summary>
    public List<(string Token, float Score)> FindMostSimilar(EmbeddingVector query, int topK = 5)
    {
        return _store.Values
            .Where(v => v.Token != query.Token)
            .Select(v => (v.Token, Score: query.CosineSimilarity(v)))
            .OrderByDescending(x => x.Score)
            .Take(topK)
            .ToList();
    }

    /// <summary>
    /// Phép toán vector nổi tiếng: king - man + woman ≈ queen
    /// </summary>
    public List<(string Token, float Score)> VectorArithmetic(
        string positive1, string positive2, string negative, int topK = 3)
    {
        var p1 = Get(positive1) ?? throw new KeyNotFoundException(positive1);
        var p2 = Get(positive2) ?? throw new KeyNotFoundException(positive2);
        var n = Get(negative) ?? throw new KeyNotFoundException(negative);

        var result = (p1 + p2 - n).Normalize();
        return FindMostSimilar(result, topK);
    }
}


// ============================================================
// PHẦN 3: Gọi API thật — OpenAI / Ollama / Anthropic-compatible
// ============================================================

public class EmbeddingApiClient : IDisposable
{
    private readonly HttpClient _http;
    private readonly string _model;

    // Dùng với OpenAI hoặc bất kỳ API tương thích OpenAI (Ollama, LocalAI...)
    public EmbeddingApiClient(string baseUrl, string apiKey, string model = "text-embedding-3-small")
    {
        _model = model;
        _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<EmbeddingVector> GetEmbeddingAsync(string text)
    {
        var payload = new { input = text, model = _model };
        var response = await _http.PostAsJsonAsync("/v1/embeddings", payload);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<OpenAiEmbeddingResponse>();
        var values = json!.Data[0].Embedding;
        return new EmbeddingVector(text, values);
    }

    public async Task<List<EmbeddingVector>> GetEmbeddingsBatchAsync(IEnumerable<string> texts)
    {
        var payload = new { input = texts.ToArray(), model = _model };
        var response = await _http.PostAsJsonAsync("/v1/embeddings", payload);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<OpenAiEmbeddingResponse>();
        return json!.Data
            .OrderBy(d => d.Index)
            .Zip(texts, (d, t) => new EmbeddingVector(t, d.Embedding))
            .ToList();
    }

    public void Dispose() => _http.Dispose();

    // ── Response model ──
    private record OpenAiEmbeddingResponse(
        [property: JsonPropertyName("data")] List<EmbeddingData> Data);
    private record EmbeddingData(
        [property: JsonPropertyName("index")] int Index,
        [property: JsonPropertyName("embedding")] float[] Embedding);
}


// ============================================================
// PHẦN 4: Demo chạy được (không cần API key)
// ============================================================

public class ProgramDaint
{
    public static async Task Main()
    {
        Console.WriteLine("=== Word Embedding Demo ===\n");

        // --- Mock embeddings (3 chiều để dễ hiểu, thực tế là 768-3072 chiều) ---
        var store = new EmbeddingStore();

        // Mỗi chiều encode một đặc tính ngữ nghĩa:
        //   dim[0] = "royalty", dim[1] = "male", dim[2] = "adult"
        store.Add(new EmbeddingVector("king", new[] { 0.99f, 0.98f, 0.95f }));
        store.Add(new EmbeddingVector("queen", new[] { 0.99f, -0.97f, 0.95f }));
        store.Add(new EmbeddingVector("man", new[] { 0.01f, 0.99f, 0.90f }));
        store.Add(new EmbeddingVector("woman", new[] { 0.01f, -0.98f, 0.90f }));
        store.Add(new EmbeddingVector("prince", new[] { 0.90f, 0.97f, 0.20f }));
        store.Add(new EmbeddingVector("dog", new[] { -0.50f, 0.10f, -0.30f }));
        store.Add(new EmbeddingVector("cat", new[] { -0.55f, -0.05f, -0.35f }));

        // 1. In vector của từng token
        Console.WriteLine("── Vectors ──");
        foreach (var token in new[] { "king", "queen", "man" })
        {
            var v = store.Get(token)!;
            Console.WriteLine($"  {token,-8} → {v}");
        }

        // 2. Cosine similarity
        Console.WriteLine("\n── Cosine Similarity ──");
        var king = store.Get("king")!;
        var queen = store.Get("queen")!;
        var dog = store.Get("dog")!;
        Console.WriteLine($"  king ↔ queen  = {king.CosineSimilarity(queen):F4}");   // cao
        Console.WriteLine($"  king ↔ dog    = {king.CosineSimilarity(dog):F4}");     // thấp

        // 3. Tìm từ gần nhất
        Console.WriteLine("\n── Từ gần nhất với 'king' ──");
        foreach (var (token, score) in store.FindMostSimilar(king, topK: 3))
            Console.WriteLine($"  {token,-10} score={score:F4}");

        // 4. Phép toán vector: king - man + woman ≈ ?
        Console.WriteLine("\n── king - man + woman ≈ ? ──");
        foreach (var (token, score) in store.VectorArithmetic("king", "woman", "man", topK: 2))
            Console.WriteLine($"  {token,-10} score={score:F4}");

        // 5. Demo gọi API thật (comment out nếu chưa có key)
        Console.WriteLine("\n── Gọi API thật (OpenAI) ──");
        await DemoRealApiAsync();
    }

    static async Task DemoRealApiAsync()
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("  (Bỏ qua — chưa set OPENAI_API_KEY)");
            Console.WriteLine("  Set biến môi trường: $env:OPENAI_API_KEY='sk-...'");
            return;
        }

        using var client = new EmbeddingApiClient(
            baseUrl: "https://api.openai.com",
            apiKey: apiKey,
            model: "text-embedding-3-small"   // 1536 chiều, rẻ nhất
        );

        var words = new[] { "bitcoin", "ethereum", "solana", "gold", "stock" };
        Console.WriteLine($"  Lấy embedding cho: {string.Join(", ", words)}");

        var vectors = await client.GetEmbeddingsBatchAsync(words);
        var realStore = new EmbeddingStore();
        vectors.ForEach(realStore.Add);

        var btc = realStore.Get("bitcoin")!;
        Console.WriteLine($"\n  Bitcoin vector: {btc}");
        Console.WriteLine("\n  Từ gần nhất với 'bitcoin':");
        foreach (var (token, score) in realStore.FindMostSimilar(btc, topK: 4))
            Console.WriteLine($"    {token,-12} {score:F4}");
    }
}