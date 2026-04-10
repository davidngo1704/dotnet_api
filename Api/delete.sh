sudo systemctl stop my_api

sudo systemctl disable my_api

sudo rm /etc/systemd/system/my_api.service

sudo systemctl daemon-reload

sudo systemctl daemon-reexec

sudo rm -rf /var/www/my_api