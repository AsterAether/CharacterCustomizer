#!/usr/bin/env bash

if [[ $(git tag -l "$1") ]]; then
    curl -L "https://drive.google.com/uc?id=1Ej8VgsW5RgK66Btb9p74tSdHMH3p4UNb&export=download" > gdrive
    ls -al
    chmod +x ./gdrive
    mv ./CharacterCustomizer/build/build.zip "./$1.zip"
    ./gdrive -c . --service-account auth.json upload -p 1TJORmK61Z1jPMBCxgnhOl6INh4bLQHWn "./$1.zip"
fi

