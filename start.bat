@echo off
echo ========================================
echo   Systeme d'Automatisation - Demarrage
echo ========================================
echo.

REM Verifier Docker
where docker >nul 2>nul
if %errorlevel% neq 0 (
    echo [ERREUR] Docker n'est pas installe.
    echo Telechargez-le sur : https://www.docker.com/products/docker-desktop
    pause
    exit /b 1
)

echo [OK] Docker est installe
echo.

REM Verifier que Docker est demarre
docker info >nul 2>nul
if %errorlevel% neq 0 (
    echo [ERREUR] Docker n'est pas demarre.
    echo Veuillez lancer Docker Desktop.
    pause
    exit /b 1
)

echo [OK] Docker est demarre
echo.

REM Creer le fichier .env s'il n'existe pas
if not exist .env (
    echo [INFO] Creation du fichier de configuration...
    (
        echo # Configuration de test
        echo SA_PASSWORD=Test@Password123
        echo EMAIL_USERNAME=test@example.com
        echo EMAIL_PASSWORD=test
        echo TWILIO_ACCOUNT_SID=test
        echo TWILIO_AUTH_TOKEN=test
        echo CHIME_WEBHOOK_URL=https://test.example.com
        echo CORTEX_API_KEY=test
    ) > .env
    echo [OK] Fichier .env cree
) else (
    echo [OK] Fichier .env existant
)

echo.
echo [INFO] Demarrage des conteneurs Docker...
docker-compose up -d

echo.
echo [INFO] Attente du demarrage des services (30 secondes)...
timeout /t 30 /nobreak >nul

echo.
echo ========================================
echo       APPLICATION DEMARREE !
echo ========================================
echo.
echo Application Web    : http://localhost:4200
echo API Backend        : http://localhost:5000
echo Hangfire Dashboard : http://localhost:5000/hangfire
echo Swagger API        : http://localhost:5000/swagger
echo.
echo Identifiants de test :
echo   Username : admin
echo   Password : admin
echo.
echo Pour arreter : docker-compose down
echo Guide complet : voir GUIDE_CLIENT.md
echo.

REM Ouvrir le navigateur
start http://localhost:4200

pause

