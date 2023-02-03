# Bannerlord.ExtractPrices

This mod extracts trade prices from Bannerlord as csv so that we can get some Business Intelligence™.

## User Guide

Installation: unzip to `SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\Bannerlord.ExtractPrices`

Extract Destination: same as above; file will be named `town.csv` and `prices-${year}-${day}.csv`

Extract Frequency: weekly in game; an additional extract on game load

Dependency: none

Compatibility: this mod does not modify the game in anyway so is safe to enable/disable at any time

Visualization: [the viewer project](https://github.com/liqi0816/bannerlord.extractprices.view) can provide a quick explanation of the data

## Developer Guide

This mod uses [Bannerlord Module Template](https://github.com/BUTR/Bannerlord.Module.Template). It needs an environment variable `BANNERLORD_GAME_DIR` to be set.

## License

MIT
