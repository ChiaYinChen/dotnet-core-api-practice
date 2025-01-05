#!/bin/sh

DOTNET_ENV=${DOTNET_ENV:-dev}

if [ "$DOTNET_ENV" = "dev" ]; then
    echo "Starting in development mode..."
    dotnet watch run
else
    echo "Starting in production mode..."
fi
