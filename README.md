# Readme Maker
Creates a file that lists all new and modified cards/abilities and special abilities... etc to be easiy viewed or even used in your Mods' Readme.



To see what your Readme looks like when its on the store.
- Open your Readme.md in a text editor (Notepad, Sublime... etc)
- Copy everything
- Go to https://markdownlivepreview.com/
- Paste in the left column

## Supports:
- [Life Cost API](https://inscryption.thunderstore.io/package/Void_Slime/Life_Cost_API/)
- [Side Deck Selector](https://inscryption.thunderstore.io/package/Infiniscryption/Side_Deck_Selector/)

## Example mods
- [Void Life and Currency Cardpack](https://inscryption.thunderstore.io/package/Void_Slime/Void_Life_and_Currency_Cardpack/)
- [Sire MoDeers](https://inscryption.thunderstore.io/package/Sire/Sire_MoDeers/)
- [MycoBotsinAct1](https://inscryption.thunderstore.io/package/Cevin2006/MycoBotsinAct1/)
- [ZergMod](https://inscryption.thunderstore.io/package/JamesGames/ZergMod/)


## How to use it

### Setup
- Install the mod (Through thunderstore is highly recommended)
- Start the game
- Wait for the console to load the ReadmeMaker mod
- Quit game
- Change **_ReadmeMaker.Enabled** to **true** in the config
- Restart the game so it can add the rest of the configs for you 

### Making a Readme
- Start Inscryption
- Start the game either vanilla or KCM until it gets to the map
- Wait for console to say that it has dumped the GENERATED_README.md


# !NOTE!
- Thunderstore will not accept a Readme with more than 32,768 characters. So if your mod has a lot of cards and sigils it's likely you'll hit this limit.


## Adding Custom Information
The Readme Maker has support for your mod to add additional information to readme dumps

### Setup
- Go to https://github.com/JamesVeug/InscyptionReadmeMaker/tree/develop/Scripts
- Download ExternalHelpers method and put it in your mod

### How to add a custom card section to your mod
- Create a new class and make it inherit from `CustomCardSection` 
- Override Initialize so it returns all the cards you want to appear in the readme dump.
- In your `Plugin.cs` script call `AddSectionToReadmeMaker`


eg:
```csharp
private void Awake()
{
    new AllBoneCardsSection().AddSectionToReadmeMaker();
}
```

```csharp
public class AllBoneCardsSection : CustomCardSection
{
    public override string SectionName() => "All Bone Cards";
    public override bool Enabled() => true;
    
    public override List<CardInfo> Initialize()
    {
        return CardManager.AllCardsCopy.FindAll(a => a.bonesCost > 0);
    }
}
```

### How to add a general custom section to your mod
[Go here for an example of what a custom section looks like](https://github.com/JamesVeug/InscyptionReadmeMaker/blob/develop/Scripts/ExternalHelpers/Examples/ExampleCustomSection.cs)
- Create a new class and make it inherit from `CustomSection`
- Fill our all methods as you need
- In your `Plugin.cs` script call `AddSectionToReadmeMaker`

eg:
```csharp
private void Awake()
{
    new ExampleCustomSection().AddSectionToReadmeMaker();
}
```

### How to add a custom cost to your mod
[Go here for an example of what a custom cost looks like](https://github.com/JamesVeug/InscyptionReadmeMaker/blob/develop/Scripts/ExternalHelpers/Examples/ExampleCustomCost.cs)
- Create a new class and make it inherit from `CustomCost`
- Fill our all methods as you need
- In your `Plugin.cs` script call `AddCostToReadmeMaker`

eg:
```csharp
private void Awake()
{
    new ExampleCustomCost().AddCostToReadmeMaker();
}
```

### Includes:

<details>
<summary>43 New Configs:
</summary>

|Section|Key|Description|
|:-|:-|:-|
|1. Readme Maker|Enabled|Should the ReadmeMaker create a GeneratedReadme?|
|1. Readme Maker|Save To|Where to save the generated readme to. If blank will be same folder as ReadmeMaker.dll. See console for exact location after making a readme.|
|2. General|Display By|Changes how the cards, abilities and special abilities are displayed.|
|2. General|Filter by JSONLoader Mod Prefix|Show .jdlr cards with a specific Mod Prefix. Disable by leaving blank.|
|2. General|Filter by Mod GUID|Only cards, sigils... etc related to this mods GUID. Disable by leaving blank.|
|2. General|Header Size|How big should the header be? (Does not work for type Foldout!|
|2. General|Header Type|How should the header be shown? (Unaffected by Size)|
|2. General|Ignore Empty Columns|True=Any columns that have no data will not be shown, False=All columns shown even if it has no data to show.|
|2. General|Ignore Mod by GUID|Ignore mods using these guids. Separate multiple guids by a comma. Disable by leaving blank.|
|2. General|Show GUIDs|Show the mod GUID for each sigils, tribes... etc.|
|2. General|Sort By|Changes the order of how rows in sections are displayed.|
|2. General|Sort by Ascending|True=Names will be ordered from A-Z, False=Z-A... etc.|
|3. Toggle Sections|Gramophone Sort Type|Order of which the Gramophone tracks will show in.|
|3. Toggle Sections|Show Boons|Show all new Boons added in its own section..|
|3. Toggle Sections|Show Cards Modified|Show a section that lists all the cards modified.|
|3. Toggle Sections|Show Configs|Should the Readme Maker show a section listing all the new configs added?|
|3. Toggle Sections|Show Consumable Items|Show all new Consumable Items added in its own section.|
|3. Toggle Sections|Show Encounters|Show all new encounters added in its own section..|
|3. Toggle Sections|Show Gramophone Tracks|Show all new Gramophone Tracks added in its own section.|
|3. Toggle Sections|Show Kaycees Mod Challenges|Show all new challenges added for Kaycee's mod.|
|3. Toggle Sections|Show Kaycees Mod Starter Decks|Show all new starter decks for Kaycee's mod.|
|3. Toggle Sections|Show Map Nodes|Show all new map nodes added in its own section..|
|3. Toggle Sections|Show Pelts|Show all new Pelts added in its own section.|
|3. Toggle Sections|Show Regions|Show all new regions.|
|3. Toggle Sections|Show Side Decks|Show a section that lists all the custom side deck cards.|
|3. Toggle Sections|Show Sigils|Show all new sigils listed on cards in its own section.|
|3. Toggle Sections|Show Special Abilities|Show all new special abilities listed on cards in its own section.|
|3. Toggle Sections|Show Tribes|Show all new tribes added in its own section.|
|3. Toggle Sections|Show modified only Changes|When True this section will only show what has changed on cards.|
|4. Card Section Options|Align Cost|Centers the cost of the costs. (Adds a lot of characters)|
|4. Card Section Options|Join duplicate Sigils|If a card has 2 of the same sigil, it will show as Fledgling(x2) instead of Fledgling, Fledgling.|
|4. Card Section Options|Show Cost Min Collapse Amount|Minimum amount before costs are shown as (icon)5 instead of (icon)(icon)...etc|
|4. Card Section Options|Show Evolutions|Show what each card can evolve into when given Fledgling. (Wolf Cub -> Wolf, Elf Fawn -> Elf... etc).|
|4. Card Section Options|Show Frozen Away|Show what each card turns into when killed given the Frozen Away sigil. (Frozen Possum -> Possum... etc).|
|4. Card Section Options|Show Meta Categories|Show what meta categories a card has. These indicate how the player can obtain the card|
|4. Card Section Options|Show Sigils|Show what each cards Sigils are. (Waterborne, Fledgling... etc).|
|4. Card Section Options|Show Specials|Show what each cards Special Abilities are. (Ouroboros, Mirror, CardsInHand... etc).|
|4. Card Section Options|Show Tail|Show what each card will leave behind before attacked. (Skink -> Skink Tail... etc).|
|4. Card Section Options|Show Traits|Show what Traits each card has (KillSurvivors, Ant, Goat, Pelt, Terrain... etc).|
|4. Card Section Options|Show Tribes|Show what Tribes each card has (Insect, Canine... etc).|
|4. Card Section Options|Show Unobtainable Cards|Show cards that can not be added to your deck.  (Trail cards, Frozen Away Cards, Evolutions... etc)|
|4. Card Section Options|Sort Type|Changes the order that the cards will be displayed in.|
|4. Card Section Options|Sort by Ascending|True=Names will be ordered from A-Z, False=Z-A... etc|
</details>




## Contributers
- **JamesGames**
- **TeamDoodz**
- **der Kartoffelcode**

## Special Thanks:
- **Eri** - Eris's Battery Cost icon
- **Manta Rain** - Manta's Battery Cost icon
- **Zepht** - Zepht's Battery Cost icon
