using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using System;
using System.Collections.Generic;
using System.IO;
using TaleWorlds.Localization;
using System.Text;

namespace Bannerlord.ExtractPrices
{
    public class SubModule : MBSubModuleBase
    {
        protected string modDir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)));

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);
            if (game.GameType is not Campaign)
                return;
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(ExtractTown));
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(ExtractPrices));
            CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(ExtractPrices));
        }

        public void ExtractTown(CampaignGameStarter _starter)
        {
            using var writer = new StreamWriter(Path.Combine(modDir, "town.csv"), append: false, encoding: Encoding.Default);
            writer.WriteLine("Town,Name,x,y");
            foreach (var town in Town.AllTowns)
            {
                var townId = TextObject.ConvertToStringList(new List<TextObject> { town.Name })[0];
                var position = town.Settlement.GetPosition2D;
                writer.WriteLine($"{townId},{town.Name},{position.x},{position.y}");
            }
            writer.Flush();
        }

        public void ExtractPrices(CampaignGameStarter _starter)
        {
            ExtractPrices();
        }

        public void ExtractPrices()
        {
            using var writer = new StreamWriter(Path.Combine(modDir, $"price-{CampaignTime.Now.GetYear}-{CampaignTime.Now.GetDayOfYear}.csv"), append: false, encoding: Encoding.Default);
            writer.WriteLine("Town,Item,Name,MSRP,Bid,Ask,Quantity,Supply,Demand,Delivered");
            var itemFilter = new HashSet<ItemObject.ItemTypeEnum> { ItemObject.ItemTypeEnum.Animal, ItemObject.ItemTypeEnum.Horse, ItemObject.ItemTypeEnum.Goods };
            foreach (var town in Town.AllTowns)
            {
                var townId = TextObject.ConvertToStringList(new List<TextObject> { town.Name })[0];
                foreach (ItemObject item in Items.All)
                {
                    if (itemFilter.Contains(item.Type))
                    {
                        var itemId = TextObject.ConvertToStringList(new List<TextObject> { item.Name })[0];
                        var market = town.MarketData;
                        var bid = market.GetPrice(item, MobileParty.MainParty, true);
                        var ask = market.GetPrice(item, MobileParty.MainParty, false);
                        var quantity = town.Settlement.ItemRoster.GetItemNumber(item);
                        var category = market.GetCategoryData(item.ItemCategory);
                        writer.WriteLine($"{townId},{itemId},{item.Name},{item.Value},{bid},{ask},{quantity},{category.Supply},{category.Demand},{category.InStoreValue}");
                    }
                }
            }
            writer.Flush();
        }
    }
}