$script:newbieQuestDatabase = @(
    @{
        Id = 0; Name = "Luyen tap"; LevelNeed = 1; NpcId = 94; MapId = 75; NpcName = "Ba Utatane"; NpcX = 579; NpcY = 334; TargetLevel = 2
        Steps = @(
            @{ Step = 0; Name = "Su dung vu khi"; Type = "equip_weapon"; ItemId = 28; WeaponIds = @(28, 29, 30, 31, 32); DurationSec = 3 }
            @{ Step = 1; Name = "Cau hinh tu dong"; Type = "config_auto"; DurationSec = 2 }
            @{ Step = 2; Name = "Danh nga bu nhin rom"; Type = "mob"; MobId = 153; MobName = "Bu nhin rom"; MapId = 75; MapName = "Lang La"; X = 900; Y = 373; Target = 5; DurationSec = 20 }
        )
    },
    @{
        Id = 1; Name = "Bat cuu ve"; LevelNeed = 2; NpcId = 94; MapId = 75; NpcName = "Ba Utatane"; NpcX = 579; NpcY = 334; TargetLevel = 4
        Steps = @(
            @{ Step = 0; Name = "Bat cuu con"; Type = "mob"; MobId = 154; MobName = "Cuu con"; MapId = 75; MapName = "Lang La"; X = 540; Y = 146; Target = 10; DropItem = 378; DurationSec = 35 }
        )
    },
    @{
        Id = 2; Name = "Hai thuoc tri thuong"; LevelNeed = 4; NpcId = 94; MapId = 75; NpcName = "Ba Utatane"; NpcX = 579; NpcY = 334; TargetLevel = 5
        Steps = @(
            @{ Step = 0; Name = "Hai cay thuoc duoc"; Type = "mob"; MobId = 143; MobName = "Cay thuoc duoc"; MapId = 75; MapName = "Lang La"; X = 1017; Y = 259; Target = 3; DropItem = 194; DurationSec = 15
               Spots = @(
                   @{ X = 1017; Y = 259 },
                   @{ X = 871; Y = 258 },
                   @{ X = 770; Y = 256 }
               )
            }
        )
    },
    @{
        Id = 3; Name = "Lam nguoi tot bung"; LevelNeed = 5; NpcId = 99; MapId = 75; NpcName = "Anko Mitarashi"; NpcX = 1127; NpcY = 141; TargetLevel = 6
        Steps = @(
            @{ Step = 0; Name = "Mua thuc an mi xao"; Type = "buy_item"; ItemId = 22; NpcId = 99; MapId = 75; X = 1127; Y = 141; DurationSec = 5 }
            @{ Step = 1; Name = "Su dung mi xao"; Type = "use_item"; ItemId = 22; DurationSec = 3 }
            @{ Step = 2; Name = "Noi chuyen voi Anko"; Type = "talk_npc"; NpcId = 99; NpcIndex = 0; MapId = 75; X = 1127; Y = 141; DurationSec = 3 }
            @{ Step = 3; Name = "Bat ran tim"; Type = "mob"; MobId = 155; MobName = "Ran tim"; MapId = 82; MapName = "Rung la phong"; X = 237; Y = 390; Target = 3; DropItem = 379; DurationSec = 25 }
        )
    },
    @{
        Id = 4; Name = "Tim Udon"; LevelNeed = 6; NpcId = 3; MapId = 75; NpcName = "Konohamaru"; NpcX = 212; NpcY = 255; TargetLevel = 7
        Steps = @(
            @{ Step = 0; Name = "Tim Udon"; Type = "talk_npc"; NpcId = 2; MapId = 82; X = 1043; Y = 114; DurationSec = 5 },
            @{ Step = 1; Name = "Dan Udon ve"; Type = "talk_npc"; NpcId = 2; MapId = 82; X = 1043; Y = 114; DurationSec = 5 }
        )
    },
    @{
        Id = 5; Name = "Xua duoi doi rung"; LevelNeed = 7; NpcId = 94; MapId = 75; NpcName = "Ba Utatane"; NpcX = 579; NpcY = 334; TargetLevel = 8
        Steps = @(
            @{ Step = 0; Name = "Duoi doi rung"; Type = "mob"; MobId = 156; MobName = "Doi rung"; MapId = 82; MapName = "Rung la phong"; X = 785; Y = 485; Target = 15; DurationSec = 45 }
        )
    },
    @{
        Id = 6; Name = "Canh bao dan lang"; LevelNeed = 8; NpcId = 94; MapId = 75; NpcName = "Ba Utatane"; NpcX = 579; NpcY = 334; TargetLevel = 9
        Steps = @(
            @{ Step = 0; Name = "Cam co vi tri 1"; Type = "use_item_at"; ItemId = 380; MapId = 83; MapName = "Thung lung Tan Cung"; X = 424; Y = 560; DurationSec = 5 }
            @{ Step = 1; Name = "Cam co vi tri 2"; Type = "use_item_at"; ItemId = 380; MapId = 83; MapName = "Thung lung Tan Cung"; X = 159; Y = 396; DurationSec = 5 }
            @{ Step = 2; Name = "Cam co vi tri 3"; Type = "use_item_at"; ItemId = 380; MapId = 83; MapName = "Thung lung Tan Cung"; X = 843; Y = 187; DurationSec = 5 }
        )
    },
    @{
        Id = 7; Name = "Xua duoi hau tu"; LevelNeed = 9; NpcId = 94; MapId = 75; NpcName = "Ba Utatane"; NpcX = 579; NpcY = 334; TargetLevel = 10
        Steps = @(
            @{ Step = 0; Name = "Duoi khi"; Type = "mob"; MobId = 157; MobName = "Khi rung"; MapId = 83; MapName = "Thung lung Tan Cung"; X = 568; Y = 730; Target = 20; DurationSec = 60 }
        )
    },
    @{
        Id = 8; Name = "Nhan Gia hoc vien"; LevelNeed = 10; NpcId = 94; MapId = 75; NpcName = "Ba Utatane"; NpcX = 579; NpcY = 334; TargetLevel = 10
        Steps = @(
            @{ Step = 0; Name = "Tu biet Konohamaru"; Type = "talk_npc"; NpcId = 3; MapId = 75; X = 212; Y = 255; DurationSec = 5 },
            @{ Step = 1; Name = "Tu biet Anko"; Type = "talk_npc"; NpcId = 99; MapId = 75; X = 1127; Y = 141; DurationSec = 5 },
            @{ Step = 2; Name = "Den Truong Konoha"; Type = "goto_map"; MapId = 86; MapName = "Truong Konoha"; X = 200; Y = 300; DurationSec = 10 },
            @{ Step = 3; Name = "Truong Konoha"; Type = "goto_map"; MapId = 86; MapName = "Truong Konoha"; X = 200; Y = 300; DurationSec = 10 }
        )
    }
)

function Get-NewbieTaskDef([int]$taskId) {
    if ($taskId -ge 0 -and $taskId -lt $script:newbieQuestDatabase.Count) {
        return $script:newbieQuestDatabase[$taskId]
    }
    return $null
}

function Get-NewbieTaskForLevel([int]$lvl) {
    if ($lvl -lt 2) { return (Get-NewbieTaskDef 0) }
    if ($lvl -lt 4) { return (Get-NewbieTaskDef 1) }
    if ($lvl -lt 5) { return (Get-NewbieTaskDef 2) }
    if ($lvl -lt 6) { return (Get-NewbieTaskDef 3) }
    if ($lvl -lt 7) { return (Get-NewbieTaskDef 4) }
    if ($lvl -lt 8) { return (Get-NewbieTaskDef 5) }
    if ($lvl -lt 9) { return (Get-NewbieTaskDef 6) }
    if ($lvl -lt 10) { return (Get-NewbieTaskDef 7) }
    return $null
}
