#!/usr/bin/env bash
pm2 start npm --name vite-dev -- run dev --max-memory-restart 1500MB

# Restart thủ công
# pm2 restart vite-dev
# pm2 restart vite-dev --update-env

# Xoá thủ công
# pm2 stop vite-dev

# Xoá hẳn khoi
# pm2 delete vite-dev

# Xem log
# pm2 logs vite-dev
# pm2 logs vite-dev --lines 100

# Xem memory/cpu
# pm2 monit

# Restart tất cả
# pm2 restart all

# Stop tất cả
# pm2 stop all

# Lưu state để auto-start khi reboot.
# pm2 savew