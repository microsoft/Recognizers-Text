// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// ------------------------------------------------------------------------------

import { BaseNumbers } from "./baseNumbers";
export namespace JapaneseNumericWithUnit {
    export const AgeAmbiguousValues = [ "歳" ];
    export const AgeSuffixList: ReadonlyMap<string, string> = new Map<string, string>([["Year", "歳"],["Month", "ヶ月"],["Week", "週間|週"],["Day", "日間|日齢|日大"]]);
    export const BuildPrefix = '';
    export const BuildSuffix = '';
    export const ConnectorToken = '';
    export const CurrencySuffixList: ReadonlyMap<string, string> = new Map<string, string>([["Afghan afghani", "アフガニ"],["Pul", "プル"],["Euro", "ユーロ"],["Cent", "セント"],["Albanian lek", "アルバニアレク|アルバニア・レク|レク"],["Angolan kwanza", "アンゴラクワンザ|アンゴラ・クワンザ|クワンザ"],["Armenian dram", "アルメニアドラム|アルメニア・ドラム|ドラム"],["Aruban florin", "アルバ・フロリン|フロリン"],["Bangladeshi taka", "タカ|バングラデシュ・タカ"],["Paisa", "パイサ"],["Bhutanese ngultrum", "ニュルタム|ブータン・ニュルタム|ブータンニュルタム"],["Chetrum", "チェルタム"],["Bolivian boliviano", "ボリビアーノ"],["Bosnia and Herzegovina convertible mark", "兌換マルク"],["Botswana pula", "ボツワナ・プラ|ボツワナプラ|プラ"],["Thebe", "テベ"],["Brazilian real", "ブラジル・レアル|ブラジルレアル|レアル"],["Bulgarian lev", "ブルガリア・レフ|ブルガリアレフ|レフ"],["Stotinka", "ストティンカ"],["Cambodian riel", "カンボジア・リエル|カンボジアリエル|リエル"],["Cape Verdean escudo", "カーボベルデ・エスクード"],["Croatian kuna", "クロアチアクーナ|クロアチア・クーナ|クーナ"],["Lipa", "リパ"],["Eritrean nakfa", "エリトリア・ナクファ|エリトリアナクファ|ナクファ"],["Ethiopian birr", "エチオピア・ブル|エチオピアブル|ブル"],["Gambian dalasi", "ガンビア・ダラシ|ガンビアダラシ|ダラシ"],["Butut", "ブトゥツ"],["Georgian lari", "ジョージア・ラリ|ジョージアラリ|ラリ"],["Tetri", "テトリ"],["Ghanaian cedi", "ガーナ・セディ|ガーナセディ|セディ"],["Pesewa", "ペセワ"],["Guatemalan quetzal", "グアテマラ・ケツァル|グアテマラケツァル|ケツァル"],["Haitian gourde", "ハイチ・グールド|ハイチグールド|グールド"],["Honduran lempira", "ホンジュラス・レンピラ|ホンジュラスレンピラ|レンピラ"],["Hungarian forint", "ハンガリー・フォリント|ハンガリーフォリント|フォリント"],["Iranian rial", "イラン・リアル"],["Yemeni rial", "イエメン・リアル"],["Israeli new shekel", "₪|ils|イスラエル・新シェケル|イスラエル新シェケル"],["Japanese yen", "円"],["Sen", "銭"],["Kazakhstani tenge", "テンゲ|カザフスタン・テンゲ|カザフスタンテンゲ"],["Kenyan shilling", "ケニア・シリング"],["North Korean won", "北朝鮮ウォン"],["South Korean won", "韓国ウォン"],["Korean won", "₩"],["Kyrgyzstani som", "キルギス・ソム|ソム"],["Lao kip", "キップ|ラオス・キップ|ラオスキップ"],["Att", "att"],["Lesotho loti", "ロチ|レソト・ロチ|レソトロチ"],["South African rand", "ランド|南アフリカ・ランド|南アフリカランド"],["Macedonian denar", "マケドニア・デナール"],["Deni", "デニ"],["Malagasy ariary", "アリアリ|マダガスカル・アリアリ|マダガスカルアリアリ"],["Iraimbilanja", "イライムビランジャ"],["Malawian kwacha", "マラウイ・クワチャ"],["Tambala", "タンバラ"],["Malaysian ringgit", "リンギット|マレーシア・リンギット"],["Mauritanian ouguiya", "ウギア|モーリタニア・ウギア|モーリタニアウギア"],["Khoums", "コウム"],["Mozambican metical", "メティカル|モザンビーク・メティカル|モザンビークメティカル"],["Burmese kyat", "チャット|ミャンマー・チャット|ミャンマーチャット"],["Pya", "ピャー"],["Nigerian naira", "ナイラ|ナイジェリア・ナイラ|ナイジェリアナイラ"],["Kobo", "コボ"],["Turkish lira", "トルコリラ"],["Kuruş", "クルシュ"],["Omani rial", "オマーン・リアル"],["Panamanian balboa", "バルボア|パナマ・バルボア|パナマバルボア"],["Centesimo", "センテシモ"],["Papua New Guinean kina", "キナ|パプア・ニューギニア・キナ"],["Toea", "トエア"],["Peruvian sol", "ヌエボ・ソル"],["Polish złoty", "ズウォティ|ポーランド・ズウォティ|ポーランドズウォティ"],["Grosz", "グロシュ"],["Qatari riyal", "カタール・リヤル"],["Saudi riyal", "サウジアラビア・リヤル"],["Riyal", "リヤル"],["Dirham", "ディルハム"],["Halala", "ハララ"],["Samoan tālā", "タラ|サモア・タラ|サモアタラ"],["Sierra Leonean leone", "レオン|シエラレオネ・レオン|シエラレオネレオン"],["Peseta", "ユーロ"],["Swazi lilangeni", "リランゲニ|スワジランド・リランゲニ|スワジランドリランゲニ"],["Tajikistani somoni", "ソモニ|タジキスタン・ソモニ|タジキスタンソモニ"],["Thai baht", "バーツ|タイ・バーツ|タイバーツ"],["Satang", "サタン"],["Tongan paʻanga", "パアンガ|トンガ・パアンガ|トンガパアンガ"],["Ukrainian hryvnia", "フリヴニャ|ウクライナ・フリヴニャ|ウクライナフリヴニャ"],["Vanuatu vatu", "バツ|バヌアツ・バツ|バヌアツバツ"],["Vietnamese dong", "ドン|ベトナム・ドン|ベトナムドン"],["Indonesian rupiah", "ルピア|インドネシア・ルピア|インドネシアルピア"],["Netherlands guilder", "ユーロ|オランダ・ユーロ"],["Surinam florin", "スリナム・ドル"],["Zambian kwacha", "ザンビア・クワチャ"],["Moroccan dirham", "モロッコ・ディルハム"],["United Arab Emirates dirham", "UAEディルハム"],["Azerbaijani manat", "アゼルバイジャン・マナト"],["Turkmenistan manat", "トルクメニスタン・マナト"],["Manat", "マナト"],["Somali shilling", "ソマリア・シリング"],["Somaliland shilling", "ソマリランド・シリング"],["Tanzanian shilling", "タンザニア・シリング"],["Ugandan shilling", "ウガンダ・シリング"],["Romanian leu", "ルーマニア・レウ"],["Moldovan leu", "モルドバ・レウ"],["Leu", "レウ"],["Ban", "バン"],["Nepalese rupee", "ネパール・ルピー"],["Pakistani rupee", "パキスタン・ルピー"],["Indian rupee", "インド・ルピー"],["Seychellois rupee", "セーシェル・ルピー"],["Mauritian rupee", "モーリシャス・ルピー"],["Maldivian rufiyaa", "ルフィヤ|モルディブ・ルフィヤ|モルディブルフィヤ"],["Sri Lankan rupee", "スリランカ・ルピー"],["Rupee", "ルピー"],["Czech koruna", "チェコ・コルナ"],["Danish krone", "デンマーク・クローネ"],["Norwegian krone", "ノルウェー・クローネ"],["Faroese króna", "フェロー・クローネ"],["Icelandic króna", "アイスランド・クローナ"],["Swedish krona", "スウェーデン・クローナ"],["Krone", "クローナ"],["Øre", "オーレ"],["West African CFA franc", "CFAフラン"],["Central African CFA franc", "CFAフラン"],["Comorian franc", "コモロ・フラン"],["Congolese franc", "コンゴ・フラン"],["Burundian franc", "ブルンジ・フラン"],["Djiboutian franc", "ジブチ・フラン"],["CFP franc", "CFPフラン"],["Guinean franc", "ギニア・フラン"],["Swiss franc", "スイス・フラン"],["Rwandan franc", "ルワンダ・フラン"],["Belgian franc", "ベルギー・フラン"],["Rappen", "Rappen"],["Franc", "フラン"],["Centime", "サンチーム"],["Russian ruble", "ロシア・ルーブル"],["Transnistrian ruble", "沿ドニエストル・ルーブル"],["Belarusian ruble", "ベラルーシ・ルーブル"],["Kopek", "カペイカ"],["Ruble", "ルーブル"],["Algerian dinar", "アルジェリア・ディナール"],["Bahraini dinar", "バーレーン・ディナール"],["Iraqi dinar", "イラク・ディナール"],["Jordanian dinar", "ヨルダン・ディナール"],["Kuwaiti dinar", "クウェート・ディナール"],["Libyan dinar", "リビア・ディナール"],["Serbian dinar", "セルビア・ディナール"],["Tunisian dinar", "チュニジア・ディナール"],["Dinar", "ディナール"],["Fils", "フィルス"],["Para", "パラ"],["Millime", "ミリム"],["Argentine peso", "ペソ|アルゼンチン・ペソ"],["Chilean peso", "チリ・ペソ"],["Colombian peso", "コロンビア・ペソ"],["Cuban peso", "兌換ペソ"],["Dominican peso", "ドミニカ・ペソ"],["Mexican peso", "メキシコ・ペソ"],["Philippine peso", "フィリピン・ペソ"],["Uruguayan peso", "ウルグアイ・ペソ"],["Peso", "ペソ"],["Centavo", "センターボ"],["Alderney pound", "ガーンジー・ポンド"],["British pound", "UKポンド"],["Guernsey pound", "ガーンジー・ポンド"],["Saint Helena pound", "セントヘレナ・ポンド"],["Egyptian pound", "エジプト・ポンド"],["Falkland Islands pound", "フォークランド諸島ポンド"],["Gibraltar pound", "ジブラルタル・ポンド"],["Manx pound", "マン島ポンド"],["Jersey pound", "ジャージー・ポンド"],["Lebanese pound", "レバノン・ポンド"],["South Sudanese pound", "南スーダン・ポンド"],["Sudanese pound", "スーダン・ポンド"],["Syrian pound", "シリア・ポンド"],["Pound", "ポンド"],["Pence", "ペニー"],["Shilling", "シリング"],["United States dollar", "ドル|USドル"],["East Caribbean dollar", "東カリブ・ドル"],["Australian dollar", "オーストラリア・ドル"],["Bahamian dollar", "バハマ・ドル"],["Barbadian dollar", "バルバドス・ドル"],["Belize dollar", "ベリーズ・ドル"],["Bermudian dollar", "バミューダ・ドル"],["Brunei dollar", "ブルネイ・ドル"],["Singapore dollar", "シンガポール・ドル"],["Canadian dollar", "カナダ・ドル"],["Cayman Islands dollar", "ケイマン諸島・ドル"],["New Zealand dollar", "ニュージーランド・ドル"],["Cook Islands dollar", "ニュージーランド・ドル|ニュージーランド・ドル"],["Fijian dollar", "フィジー・ドル|フィジー・ドル"],["Guyanese dollar", "ガイアナ・ドル|ガイアナ・ドル"],["Hong Kong dollar", "香港ドル"],["Macau Pataca", "マカオ・パタカ|マカオ・パタカ"],["New Taiwan dollar", "ニュー台湾ドル|ニュー台湾ドル"],["Jamaican dollar", "ジャマイカ・ドル|ジャマイカドル"],["Kiribati dollar", "オーストラリア・ドル|オーストラリアドル"],["Liberian dollar", "リベリア・ドル|リベリアドル"],["Namibian dollar", "ナミビア・ドル|ナミビアドル"],["Surinamese dollar", "スリナム・ドル|スリナムドル"],["Trinidad and Tobago dollar", "トリニダード・トバゴ・ドル|トリニダードトバゴ・ドル"],["Tuvaluan dollar", "ツバル・ドル|ツバルドル"],["Chinese yuan", "人民元"],["Fen", "分"],["Jiao", "角"],["Finnish markka", "フィンランド・マルカ"],["Penni", "ペニー"]]);
    export const CurrencyNameToIsoCodeMap: ReadonlyMap<string, string> = new Map<string, string>([["Afghan afghani", "AFN"],["Euro", "EUR"],["Albanian lek", "ALL"],["Angolan kwanza", "AOA"],["Armenian dram", "AMD"],["Aruban florin", "AWG"],["Bangladeshi taka", "BDT"],["Bhutanese ngultrum", "BTN"],["Bolivian boliviano", "BOB"],["Bosnia and Herzegovina convertible mark", "BAM"],["Botswana pula", "BWP"],["Brazilian real", "BRL"],["Bulgarian lev", "BGN"],["Cambodian riel", "KHR"],["Cape Verdean escudo", "CVE"],["Costa Rican colón", "CRC"],["Croatian kuna", "HRK"],["Czech koruna", "CZK"],["Eritrean nakfa", "ERN"],["Ethiopian birr", "ETB"],["Gambian dalasi", "GMD"],["Georgian lari", "GEL"],["Ghanaian cedi", "GHS"],["Guatemalan quetzal", "GTQ"],["Haitian gourde", "HTG"],["Honduran lempira", "HNL"],["Hungarian forint", "HUF"],["Iranian rial", "IRR"],["Yemeni rial", "YER"],["Israeli new shekel", "ILS"],["Japanese yen", "JPY"],["Kazakhstani tenge", "KZT"],["Kenyan shilling", "KES"],["North Korean won", "KPW"],["South Korean won", "KRW"],["Kyrgyzstani som", "KGS"],["Lao kip", "LAK"],["Lesotho loti", "LSL"],["South African rand", "ZAR"],["Macanese pataca", "MOP"],["Macedonian denar", "MKD"],["Malagasy ariary", "MGA"],["Malawian kwacha", "MWK"],["Malaysian ringgit", "MYR"],["Mauritanian ouguiya", "MRO"],["Mongolian tögrög", "MNT"],["Mozambican metical", "MZN"],["Burmese kyat", "MMK"],["Nicaraguan córdoba", "NIO"],["Nigerian naira", "NGN"],["Turkish lira", "TRY"],["Omani rial", "OMR"],["Panamanian balboa", "PAB"],["Papua New Guinean kina", "PGK"],["Paraguayan guaraní", "PYG"],["Peruvian sol", "PEN"],["Polish złoty", "PLN"],["Qatari riyal", "QAR"],["Saudi riyal", "SAR"],["Samoan tālā", "WST"],["São Tomé and Príncipe dobra", "STD"],["Sierra Leonean leone", "SLL"],["Swazi lilangeni", "SZL"],["Tajikistani somoni", "TJS"],["Thai baht", "THB"],["Ukrainian hryvnia", "UAH"],["Vanuatu vatu", "VUV"],["Venezuelan bolívar", "VEF"],["Zambian kwacha", "ZMW"],["Moroccan dirham", "MAD"],["United Arab Emirates dirham", "AED"],["Azerbaijani manat", "AZN"],["Turkmenistan manat", "TMT"],["Somali shilling", "SOS"],["Tanzanian shilling", "TZS"],["Ugandan shilling", "UGX"],["Romanian leu", "RON"],["Moldovan leu", "MDL"],["Nepalese rupee", "NPR"],["Pakistani rupee", "PKR"],["Indian rupee", "INR"],["Seychellois rupee", "SCR"],["Mauritian rupee", "MUR"],["Maldivian rufiyaa", "MVR"],["Sri Lankan rupee", "LKR"],["Indonesian rupiah", "IDR"],["Danish krone", "DKK"],["Norwegian krone", "NOK"],["Icelandic króna", "ISK"],["Swedish krona", "SEK"],["West African CFA franc", "XOF"],["Central African CFA franc", "XAF"],["Comorian franc", "KMF"],["Congolese franc", "CDF"],["Burundian franc", "BIF"],["Djiboutian franc", "DJF"],["CFP franc", "XPF"],["Guinean franc", "GNF"],["Swiss franc", "CHF"],["Rwandan franc", "RWF"],["Russian ruble", "RUB"],["Transnistrian ruble", "PRB"],["Belarusian ruble", "BYN"],["Algerian dinar", "DZD"],["Bahraini dinar", "BHD"],["Iraqi dinar", "IQD"],["Jordanian dinar", "JOD"],["Kuwaiti dinar", "KWD"],["Libyan dinar", "LYD"],["Serbian dinar", "RSD"],["Tunisian dinar", "TND"],["Argentine peso", "ARS"],["Chilean peso", "CLP"],["Colombian peso", "COP"],["Cuban convertible peso", "CUC"],["Cuban peso", "CUP"],["Dominican peso", "DOP"],["Mexican peso", "MXN"],["Uruguayan peso", "UYU"],["British pound", "GBP"],["Saint Helena pound", "SHP"],["Egyptian pound", "EGP"],["Falkland Islands pound", "FKP"],["Gibraltar pound", "GIP"],["Manx pound", "IMP"],["Jersey pound", "JEP"],["Lebanese pound", "LBP"],["South Sudanese pound", "SSP"],["Sudanese pound", "SDG"],["Syrian pound", "SYP"],["United States dollar", "USD"],["Australian dollar", "AUD"],["Bahamian dollar", "BSD"],["Barbadian dollar", "BBD"],["Belize dollar", "BZD"],["Bermudian dollar", "BMD"],["Brunei dollar", "BND"],["Singapore dollar", "SGD"],["Canadian dollar", "CAD"],["Cayman Islands dollar", "KYD"],["New Zealand dollar", "NZD"],["Fijian dollar", "FJD"],["Guyanese dollar", "GYD"],["Hong Kong dollar", "HKD"],["Jamaican dollar", "JMD"],["Liberian dollar", "LRD"],["Namibian dollar", "NAD"],["Solomon Islands dollar", "SBD"],["Surinamese dollar", "SRD"],["New Taiwan dollar", "TWD"],["Trinidad and Tobago dollar", "TTD"],["Tuvaluan dollar", "TVD"],["Chinese yuan", "CNY"],["Rial", "__RI"],["Shiling", "__S"],["Som", "__SO"],["Dirham", "__DR"],["Dinar", "_DN"],["Dollar", "__D"],["Manat", "__MA"],["Rupee", "__R"],["Krone", "__K"],["Krona", "__K"],["Crown", "__K"],["Frank", "__F"],["Mark", "__M"],["Ruble", "__RB"],["Peso", "__PE"],["Pound", "__P"],["Tristan da Cunha pound", "_TP"],["South Georgia and the South Sandwich Islands pound", "_SP"],["Somaliland shilling", "_SS"],["Pitcairn Islands dollar", "_PND"],["Palauan dollar", "_PD"],["Niue dollar", "_NID"],["Nauruan dollar", "_ND"],["Micronesian dollar", "_MD"],["Kiribati dollar", "_KID"],["Guernsey pound", "_GGP"],["Faroese króna", "_FOK"],["Cook Islands dollar", "_CKD"],["British Virgin Islands dollar", "_BD"],["Ascension pound", "_AP"],["Alderney pound", "_ALP"],["Abkhazian apsar", "_AA"]]);
    export const FractionalUnitNameToCodeMap: ReadonlyMap<string, string> = new Map<string, string>([["Jiao", "JIAO"],["Kopek", "KOPEK"],["Pul", "PUL"],["Cent", "CENT"],["Qindarkë", "QINDARKE"],["Penny", "PENNY"],["Santeem", "SANTEEM"],["Cêntimo", "CENTIMO"],["Centavo", "CENTAVO"],["Luma", "LUMA"],["Qəpik", "QƏPIK"],["Fils", "FILS"],["Poisha", "POISHA"],["Kapyeyka", "KAPYEYKA"],["Centime", "CENTIME"],["Chetrum", "CHETRUM"],["Paisa", "PAISA"],["Fening", "FENING"],["Thebe", "THEBE"],["Sen", "SEN"],["Stotinka", "STOTINKA"],["Fen", "FEN"],["Céntimo", "CENTIMO"],["Lipa", "LIPA"],["Haléř", "HALER"],["Øre", "ØRE"],["Piastre", "PIASTRE"],["Santim", "SANTIM"],["Oyra", "OYRA"],["Butut", "BUTUT"],["Tetri", "TETRI"],["Pesewa", "PESEWA"],["Fillér", "FILLER"],["Eyrir", "EYRIR"],["Dinar", "DINAR"],["Agora", "AGORA"],["Tïın", "TIIN"],["Chon", "CHON"],["Jeon", "JEON"],["Tyiyn", "TYIYN"],["Att", "ATT"],["Sente", "SENTE"],["Dirham", "DIRHAM"],["Rappen", "RAPPEN"],["Avo", "AVO"],["Deni", "DENI"],["Iraimbilanja", "IRAIMBILANJA"],["Tambala", "TAMBALA"],["Laari", "LAARI"],["Khoums", "KHOUMS"],["Ban", "BAN"],["Möngö", "MONGO"],["Pya", "PYA"],["Kobo", "KOBO"],["Kuruş", "KURUS"],["Baisa", "BAISA"],["Centésimo", "CENTESIMO"],["Toea", "TOEA"],["Sentimo", "SENTIMO"],["Grosz", "GROSZ"],["Sene", "SENE"],["Halala", "HALALA"],["Para", "PARA"],["Öre", "ORE"],["Diram", "DIRAM"],["Satang", "SATANG"],["Seniti", "SENITI"],["Millime", "MILLIME"],["Tennesi", "TENNESI"],["Kopiyka", "KOPIYKA"],["Tiyin", "TIYIN"],["Hào", "HAO"],["Ngwee", "NGWEE"]]);
    export const CompoundUnitConnectorRegex = `(?<spacer>と)`;
    export const CurrencyPrefixList: ReadonlyMap<string, string> = new Map<string, string>([["Dollar", "$"],["United States dollar", "us$"],["British Virgin Islands dollar", "bvi$"],["Brunei dollar", "b$"],["Sen", "sen"],["Singapore dollar", "s$"],["Canadian dollar", "can$|c$|c $"],["Cayman Islands dollar", "ci$"],["New Zealand dollar", "nz$|nz $"],["Guyanese dollar", "gy$|gy $|g$|g $"],["Hong Kong dollar", "hk$|hkd|hk $"],["Jamaican dollar", "j$"],["Namibian dollar", "nad|n$|n $"],["Solomon Islands dollar", "si$|si $"],["New Taiwan dollar", "nt$|nt $"],["Samoan tālā", "ws$"],["Chinese yuan", "￥"],["Japanese yen", "¥|\\"],["Turkish lira", "₺"],["Euro", "€"],["Pound", "£"],["Costa Rican colón", "₡"]]);
    export const CurrencyAmbiguousValues = [ "円","銭","\\" ];
}
