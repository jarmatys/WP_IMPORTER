Komenda do wyłączenia śledzenia pliku appsettings.json:

git update-index --assume-unchanged .\appsettings.json

i w drugą stronę:

git update-index --no-assume-unchanged.\appsettings.json
