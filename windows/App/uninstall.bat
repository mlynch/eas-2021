@echo off
setlocal
set APP_NAME=capacitor-app
set APP_USER_DATA_FOLDER=%AppData%\%APP_NAME%
rmdir /s /q "%APP_USER_DATA_FOLDER%"
endlocal
