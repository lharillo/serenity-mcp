#!/bin/bash
set -e

# Configuration
VERSION="1.3.1"
IMAGE_NAME="lharillo/serenity-mcp"
BUILD_DATE=$(date -u +"%Y-%m-%dT%H:%M:%SZ")
VCS_REF=$(git rev-parse --short HEAD 2>/dev/null || echo "unknown")

echo "============================================"
echo "Building Serenity Star MCP Server"
echo "============================================"
echo "Version: $VERSION"
echo "Build Date: $BUILD_DATE"
echo "VCS Ref: $VCS_REF"
echo "============================================"

# Build the Docker image with labels
docker build \
  --build-arg VERSION="$VERSION" \
  --build-arg BUILD_DATE="$BUILD_DATE" \
  --build-arg VCS_REF="$VCS_REF" \
  -t "${IMAGE_NAME}:latest" \
  -t "${IMAGE_NAME}:${VERSION}" \
  -t "${IMAGE_NAME}:$(echo $VERSION | cut -d. -f1-2)" \
  -t "${IMAGE_NAME}:$(echo $VERSION | cut -d. -f1)" \
  .

echo ""
echo "✅ Build completed successfully!"
echo ""
echo "Available tags:"
echo "  - ${IMAGE_NAME}:latest"
echo "  - ${IMAGE_NAME}:${VERSION}"
echo "  - ${IMAGE_NAME}:$(echo $VERSION | cut -d. -f1-2)"
echo "  - ${IMAGE_NAME}:$(echo $VERSION | cut -d. -f1)"
echo ""

# Ask for Docker Hub login if not already logged in
if ! docker info | grep -q "Username:"; then
    echo "Logging into Docker Hub..."
    read -p "Docker Hub username [lharillo]: " DOCKER_USER
    DOCKER_USER=${DOCKER_USER:-lharillo}
    
    read -sp "Docker Hub password/token: " DOCKER_PASS
    echo ""
    
    echo "$DOCKER_PASS" | docker login -u "$DOCKER_USER" --password-stdin
fi

# Push all tags
echo ""
echo "Publishing to Docker Hub..."
docker push "${IMAGE_NAME}:latest"
docker push "${IMAGE_NAME}:${VERSION}"
docker push "${IMAGE_NAME}:$(echo $VERSION | cut -d. -f1-2)"
docker push "${IMAGE_NAME}:$(echo $VERSION | cut -d. -f1)"

echo ""
echo "============================================"
echo "✅ Successfully published to Docker Hub!"
echo "============================================"
echo ""
echo "Pull with:"
echo "  docker pull ${IMAGE_NAME}:latest"
echo "  docker pull ${IMAGE_NAME}:${VERSION}"
echo ""
echo "View on Docker Hub:"
echo "  https://hub.docker.com/r/${IMAGE_NAME}"
echo ""
