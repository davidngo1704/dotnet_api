#!/bin/bash

set -e  # chết là chết luôn, không giả vờ tiếp tục

### ===== CONFIG =====
APP_NAME="my_api"
PROJECT_PATH="/var/lib/ApiGateway/dotnet_api/Api"   # thư mục chứa .csproj
PUBLISH_DIR="$PROJECT_PATH/out"
DEPLOY_DIR="/var/www/$APP_NAME"
DLL_NAME="Api.dll"
SERVICE_FILE="/etc/systemd/system/$APP_NAME.service"
DOTNET_PATH="/usr/bin/dotnet"
RUN_USER="www-data"

### ===== BUILD =====
echo "==> Publishing project..."
cd $PROJECT_PATH
dotnet publish -c Release -o $PUBLISH_DIR

### ===== PREPARE SERVER DIR =====
echo "==> Preparing deploy directory..."
sudo mkdir -p $DEPLOY_DIR
sudo rm -rf $DEPLOY_DIR/*
sudo cp -r $PUBLISH_DIR/* $DEPLOY_DIR/

### ===== PERMISSION =====
echo "==> Setting permissions..."
sudo chown -R $RUN_USER:$RUN_USER $DEPLOY_DIR
sudo chmod -R 755 $DEPLOY_DIR

### ===== CREATE SYSTEMD SERVICE =====
echo "==> Creating systemd service..."

sudo bash -c "cat > $SERVICE_FILE" <<EOF
[Unit]
Description=$APP_NAME ASP.NET Core App
After=network.target

[Service]
WorkingDirectory=$DEPLOY_DIR
ExecStart=$DOTNET_PATH $DEPLOY_DIR/$DLL_NAME
Restart=always
RestartSec=5
KillSignal=SIGINT
SyslogIdentifier=$APP_NAME
User=$RUN_USER
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
EOF

### ===== RELOAD + START =====
echo "==> Reloading systemd..."
sudo systemctl daemon-reexec
sudo systemctl daemon-reload

echo "==> Enabling service..."
sudo systemctl enable $APP_NAME

echo "==> Restarting service..."
sudo systemctl restart $APP_NAME

### ===== STATUS =====
echo "==> Service status:"
sudo systemctl status $APP_NAME --no-pager

echo "==> Done. Logs:"
echo "journalctl -u $APP_NAME -f"