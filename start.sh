#!/bin/bash

echo "🚀 Démarrage du Système d'Automatisation..."
echo ""

# Vérifier que Docker est installé
if ! command -v docker &> /dev/null; then
    echo "❌ Docker n'est pas installé. Veuillez l'installer : https://www.docker.com/products/docker-desktop"
    exit 1
fi

# Vérifier que Docker est démarré
if ! docker info &> /dev/null; then
    echo "❌ Docker n'est pas démarré. Veuillez lancer Docker Desktop."
    exit 1
fi

echo "✅ Docker est prêt"
echo ""

# Créer le fichier .env s'il n'existe pas
if [ ! -f .env ]; then
    echo "📝 Création du fichier de configuration..."
    cat > .env << EOF
# Configuration de test
SA_PASSWORD=Test@Password123
EMAIL_USERNAME=test@example.com
EMAIL_PASSWORD=test
TWILIO_ACCOUNT_SID=test
TWILIO_AUTH_TOKEN=test
CHIME_WEBHOOK_URL=https://test.example.com
CORTEX_API_KEY=test
EOF
    echo "✅ Fichier .env créé"
else
    echo "✅ Fichier .env existant"
fi

echo ""
echo "🐳 Démarrage des conteneurs Docker..."
docker-compose up -d

echo ""
echo "⏳ Attente du démarrage des services (30 secondes)..."
sleep 30

echo ""
echo "✅ Vérification des services..."

# Vérifier le backend
if curl -s http://localhost:5000/swagger > /dev/null; then
    echo "✅ Backend : OK"
else
    echo "⚠️  Backend : En cours de démarrage..."
fi

# Vérifier le frontend
if curl -s http://localhost:4200 > /dev/null; then
    echo "✅ Frontend : OK"
else
    echo "⚠️  Frontend : En cours de démarrage..."
fi

echo ""
echo "╔════════════════════════════════════════════════════════╗"
echo "║          🎉 APPLICATION DÉMARRÉE !                     ║"
echo "╚════════════════════════════════════════════════════════╝"
echo ""
echo "📱 Application Web    : http://localhost:4200"
echo "🔧 API Backend        : http://localhost:5000"
echo "📊 Hangfire Dashboard : http://localhost:5000/hangfire"
echo "📖 Swagger API        : http://localhost:5000/swagger"
echo ""
echo "🔐 Identifiants de test :"
echo "   Username : admin"
echo "   Password : admin"
echo ""
echo "📝 Pour arrêter : docker-compose down"
echo "📚 Guide complet : voir GUIDE_CLIENT.md"
echo ""

# Ouvrir le navigateur (optionnel)
if command -v xdg-open &> /dev/null; then
    xdg-open http://localhost:4200
elif command -v open &> /dev/null; then
    open http://localhost:4200
fi

