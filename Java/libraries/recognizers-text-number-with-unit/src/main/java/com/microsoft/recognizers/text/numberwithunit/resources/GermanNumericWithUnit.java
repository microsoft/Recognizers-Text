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

package com.microsoft.recognizers.text.numberwithunit.resources;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

import com.google.common.collect.ImmutableMap;

public class GermanNumericWithUnit {

    public static final ImmutableMap<String, String> AgeSuffixList = ImmutableMap.<String, String>builder()
        .put("Year", "jahr alt|jahre alt|jahren|jahre|lebensjahr")
        .put("Month", "monat alt|monate alt|monaten|monate")
        .put("Week", "woche alt|wochen alt|wochen|woche")
        .put("Day", "tag alt|tage alt|tagen|tage")
        .build();

    public static final ImmutableMap<String, String> AreaSuffixList = ImmutableMap.<String, String>builder()
        .put("Square kilometer", "qkm|quadratkilometer|km^2|km²")
        .put("Square hectometer", "qhm|quadrathektometer|hm^2|hm²|hektar")
        .put("Square decameter", "quadratdekameter|dam^2|dam²")
        .put("Square meter", "qm|quadratmeter|m^2|m²")
        .put("Square decimeter", "qdm|quadratdezimeter|dm^2|dm²")
        .put("Square centimeter", "qcm|quadratzentimeter|cm^2|cm²")
        .put("Square millimeter", "qmm|quadratmillimeter|mm^2|mm²")
        .put("Square inch", "sqin|quadratzoll|in^2|in²")
        .put("Square foot", "sqft|quadratfuß|fuß^2|fuß²|ft2|ft^2|ft²")
        .put("Square mile", "sqmi|quadratmeile|mi^2|mi²")
        .put("Square yard", "sqyd|quadratyard|yd^2|yd²")
        .put("Acre", "-acre|acre|acres")
        .build();

    public static final ImmutableMap<String, String> CurrencySuffixList = ImmutableMap.<String, String>builder()
        .put("Abkhazian apsar", "abkhazian apsar|apsars")
        .put("Afghan afghani", "afghanischer afghani|afghanische afghani|afghanischen afghani|؋|afn|afghani")
        .put("Pul", "pul")
        .put("Euro", "euro|€|eur")
        .put("Cent", "cent|-cent")
        .put("Albanian lek", "albaninischer Lek|albanische Lek|albanischen Lek")
        .put("Qindarkë", "qindarkë|qindarkës|qindarke|qindarkes")
        .put("Angolan kwanza", "angolanischer kwanza|angolanische kwanza|angolanischen kwanza|kz|aoa|kwanza|kwanzas")
        .put("Armenian dram", "armeninischer dram|armeninische dram|armeninischen dram")
        .put("Aruban florin", "Aruba-Florin|ƒ|awg")
        .put("Bangladeshi taka", "bangladesischer taka|bengalischer taka|bangladesische taka|bengalische taka|bangladesischen taka|bengalischen taka|৳|bdt|taka")
        .put("Paisa", "poisha|paisa")
        .put("Bhutanese ngultrum", "bhutanischer ngultrum|bhutanische ngultrum|bhutanischen ngultrum|nu.|btn")
        .put("Chetrum", "chetrum")
        .put("Bolivian boliviano", "bolivianischer boliviano|bolivianische boliviano|bolivianischen boliviano|bob|bs.|boliviano")
        .put("Bosnia and Herzegovina convertible mark", "bosnischer konvertible mark|bosnisch-herzegowinischer konvertible mark|bosnische konvertible mark|bosnisch-herzegowinische konvertible mark|bosnischen konvertible mark|bosnisch-herzegowinischen konvertible mark|konvertible mark|bam")
        .put("Fening", "Fening")
        .put("Botswana pula", "botswanischer pula|botswanische pula|botswanischen pula|bwp|pula")
        .put("Thebe", "thebe")
        .put("Brazilian real", "brazilianischer real|brazilianische real|brazilianischen real|r$|brl|real")
        .put("Bulgarian lev", "bulgarischer lew|bulgarische lew|bulgarischen lew|bgn|лв|lew")
        .put("Stotinka", "stotinki|stotinka")
        .put("Cambodian riel", "kambodschanischer riel|kambodschanische riel|kambodschanischen riel|khr|៛|riel")
        .put("Cape Verdean escudo", "kap-verde-escudo|cve")
        .put("Costa Rican colón", "costa-rica-colón|costa-rica-colon|crc|₡")
        .put("Salvadoran colón", "svc|el-salvador-colón|el-salvador-colon")
        .put("Céntimo", "céntimo")
        .put("Croatian kuna", "kroatischer kuna|kroatische kuna|kroatischen kuna|kn|hrk|kuna")
        .put("Lipa", "lipa")
        .put("Czech koruna", "tschechische krone|tschechischen kronen|tschechischer kronen|czk|kč")
        .put("Haléř", "haléř")
        .put("Eritrean nakfa", "eritreischer nakfa|eritreische nakfa|eritreischen nakfa|nfk|ern|nakfa")
        .put("Ethiopian birr", "äthiopischer birr|äthiopische birr|äthiopischen birr|etb")
        .put("Gambian dalasi", "gambischer dalasi|gambische dalasi|gambischen dalasi|gmd")
        .put("Butut", "bututs|butut")
        .put("Georgian lari", "georgischer lari|georgische lari|georgischen lari|lari|gel|₾")
        .put("Tetri", "tetri")
        .put("Ghanaian cedi", "ghanaischer cedi|ghanaische cedi|ghanaischen cedi|Ghana cedi|ghs|₵|gh₵")
        .put("Pesewa", "pesewas|pesewa")
        .put("Guatemalan quetzal", "guatemaltekischer quetzal|guatemaltekische quetzal|guatemaltekischen quetzal|gtq|quetzal")
        .put("Haitian gourde", "haitianischer gourde|haitianische gourde|haitianischen gourde|htg")
        .put("Honduran lempira", "honduranischer lempira|honduranische lempira|honduranischen lempira|hnl")
        .put("Hungarian forint", "ungarischer forint|ungarische forint|ungarischen forint|huf|ft|forint")
        .put("Fillér", "fillér")
        .put("Iranian rial", "iranischer rial|iranische rial|iranischen rial|irr")
        .put("Yemeni rial", "jemen-rial|yer")
        .put("Israeli new shekel", "₪|ils|agora")
        .put("Lithuanian litas", "ltl|litauischer litas|litauische litas|litauischen litas")
        .put("Japanese yen", "japaneser yen|japanese yen|japanesen yen|jpy|yen|¥")
        .put("Kazakhstani tenge", "kasachischer tenge|kasachische tenge|kasachischen tenge|kzt")
        .put("Kenyan shilling", "kenia-schilling|kes")
        .put("North Korean won", "nordkoreanischer won|nordkoreanische won|nordkoreanischen won|kpw")
        .put("South Korean won", "südkoreanischer won|südkoreanische won|südkoreanischen won|krw")
        .put("Korean won", "koreanischer won|koreanische won|koreanischen won|₩")
        .put("Kyrgyzstani som", "kirgisischer som|kirgisische som|kirgisischen som|kgs")
        .put("Uzbekitan som", "usbekischer som|usbekische som|usbekischen som|usbekischer sum|usbekische sum|usbekischen sum|usbekischer so'm|usbekische so'm|usbekischen so'm|usbekischer soum|usbekische soum|usbekischen soum|uzs")
        .put("Lao kip", "laotischer kip|laotische kip|laotischen kip|lak|₭n|₭")
        .put("Att", "att")
        .put("Lesotho loti", "lesothischer loti|lesothische loti|lesothischen loti|lsl|loti")
        .put("Sente", "sente|lisente")
        .put("South African rand", "südafrikanischer rand|südafrikanische rand|südafrikanischen rand|zar")
        .put("Macanese pataca", "macao-pataca|mop$|mop")
        .put("Avo", "avos|avo")
        .put("Macedonian denar", "mazedonischer denar|mazedonische denar|mazedonischen denar|mkd|ден")
        .put("Deni", "deni")
        .put("Malagasy ariary", "madagassischer ariary|madagassische ariary|madagassischen ariary|ariary|mga")
        .put("Iraimbilanja", "iraimbilanja")
        .put("Malawian kwacha", "malawi-kwacha|mk|mwk")
        .put("Tambala", "tambala")
        .put("Malaysian ringgit", "malaysischer ringgit|malaysische ringgit|malaysischen ringgit|rm|myr")
        .put("Mauritanian ouguiya", "mauretanischer ouguiya|mauretanische ouguiya|mauretanischen ouguiya|mro")
        .put("Khoums", "khoums")
        .put("Mongolian tögrög", "mongolischer tögrög|mongolische tögrög|mongolischen tögrög|mongolischer tugrik|mongolische tugrik|mongolischen tugrik|mnt|₮")
        .put("Mozambican metical", "mosambik-metical|mosambik metical|mt|mzn")
        .put("Burmese kyat", "myanmar-kyat|myanmar kyat|ks|mmk")
        .put("Pya", "pya")
        .put("Nicaraguan córdoba", "nicaraguanischer córdoba oro|nicaraguanische córdoba oro|nicaraguanischen córdoba oro|nicaraguanischer córdoba|nicaraguanische córdoba|nicaraguanischen córdoba|nio|córdoba|córdoba oro")
        .put("Nigerian naira", "nigerianischer naira|nigerianische naira|nigerianischen naira|naira|ngn|₦|nigeria naira")
        .put("Kobo", "kobo")
        .put("Turkish lira", "türkischer lira|türkische lira|türkischen lira|tuerkischer lira|tuerkische lira|tuerkischen lira|try|tl")
        .put("Kuruş", "kuruş")
        .put("Omani rial", "omanischer rial|omanische rial|omanischen rial|omr|ر.ع.")
        .put("Panamanian balboa", "panamaischer balboa|panamaische balboa|panamaischen balboa|b/.|pab")
        .put("Centesimo", "centesimo")
        .put("Papua New Guinean kina", "papua-neuguinea-kina|kina|pgk")
        .put("Toea", "toea")
        .put("Paraguayan guaraní", "paraguayischer guaraní|paraguayische guaraní|paraguayischen guaraní|guaraní|₲|pyg")
        .put("Peruvian sol", "peruanischer sol|peruanische sol|peruanischen sol|soles|sol")
        .put("Polish złoty", "polnischer złoty|polnische złoty|polnischen złoty|polnischer zloty|polnische zloty|polnischen zloty|zł|pln|złoty|zloty")
        .put("Grosz", "groszy|grosz|grosze")
        .put("Qatari riyal", "katar-riyal|katar riyal|qatari riyal|qar")
        .put("Saudi riyal", "saudi-riyal|sar")
        .put("Riyal", "riyal|﷼")
        .put("Dirham", "dirham|dirhem|dirhm")
        .put("Halala", "halalas|halala")
        .put("Samoan tālā", "samoanischer tala|samoanische tala|samoanischen tala|samoanischer tālā|samoanische tālā|samoanischen tālā|tālā|tala|ws$|samoa|wst|samoa-tālā|samoa-tala")
        .put("Sene", "sene")
        .put("São Tomé and Príncipe dobra", "são-toméischer dobra|são-toméische dobra|são-toméischen dobra|dobra|std")
        .put("Sierra Leonean leone", "sierra-leonischer leone|sierra-leonische leone|sierra-leonischen leone|sll|leone|le")
        .put("Peseta", "pesetas|peseta")
        .put("Netherlands guilder", "florin|antillen-gulden|niederländische-antillen-gulden|antillen gulden|ang|niederländischer gulden|niederländische gulden|niederländischen gulden|gulden|fl")
        .put("Swazi lilangeni", "swazi-lilangeni|swazi lilangeni|lilangeni|szl|swazi-emalangeni|swazi emalangeni")
        .put("Tajikistani somoni", "tadschikischer somoni|tadschikische somoni|tadschikischen somoni|tadschikistan-somoni|tadschikistan somoni|tajikischer somoni|tajikische somoni|tajikischen somoni|tajikistan-somoni|tajikistan somoni|tjs")
        .put("Diram", "dirams|diram")
        .put("Thai baht", "thailändischer baht|thailändische baht|thailändischen baht|thailaendischer baht|thailaendische baht|thailaendischen baht|thai baht|thai-baht|฿|thb")
        .put("Satang", "satang|satangs")
        .put("Tongan paʻanga", "tongaischer paʻanga|tongaische paʻanga|tongaischen paʻanga|paʻanga|tonga paʻanga|tongaischer pa'anga|tongaische pa'anga|tongaischen pa'anga|pa'anga|tonga pa'anga")
        .put("Seniti", "seniti")
        .put("Ukrainian hryvnia", "ukrainischer hrywnja|ukrainische hrywnja|ukrainischen hrywnja|hrywnja|uah|₴")
        .put("Vanuatu vatu", "vanuatu-vatu|vanuatu vatu|vatu|vuv")
        .put("Venezuelan bolívar", "venezolanischer bolívar|venezolanische bolívar|venezolanischen bolívar|bs.f.|vef")
        .put("Vietnamese dong", "vietnamesischer đồng|vietnamesische đồng|vietnamesischen đồng|vietnamesischer dong|vietnamesische dong|vietnamesischen dong|vnd|đồng")
        .put("Zambian kwacha", "sambischer kwacha|sambische kwacha|sambischen kwacha|zk|zmw")
        .put("Moroccan dirham", "marokkanischer dirham|marokkanische dirham|marokkanischen dirham|mad|د.م.")
        .put("United Arab Emirates dirham", "vae dirham|vae-dirham|dirham der vereinigten arabischen emirate|د.إ|aed")
        .put("Azerbaijani manat", "aserbaidschan-manat|azn")
        .put("Turkmenistan manat", "turkmenistan-manat|tmt")
        .put("Manat", "manat")
        .put("Qəpik", "qəpik")
        .put("Somali shilling", "somalia-schilling|sh.so.|sos")
        .put("Somaliland shilling", "somaliland-schilling")
        .put("Tanzanian shilling", "tansania-schilling|tsh|tzs")
        .put("Ugandan shilling", "uganda-schilling|ugx")
        .put("Romanian leu", "rumänischer leu|rumänische leu|rumänischen leu|rumaenischer leu|rumaenische leu|rumaenischen leu|lei|ron")
        .put("Moldovan leu", "moldauischer leu|moldauische leu|moldauischen leu|mdl|moldau leu")
        .put("Leu", "leu")
        .put("Ban", "bani|ban")
        .put("Nepalese rupee", "nepalesischer rupie|nepalesische rupie|nepalesischen rupie|nepalesische rupien|nepalesischer rupien|nepalesischen rupien|npr")
        .put("Pakistani rupee", "pakistanischer rupie|pakistanische rupie|pakistanischen rupie|pakistanischer rupien|pakistanische rupien|pakistanischen rupien|pkr")
        .put("Indian rupee", "indischer rupie|indische rupie|indischen rupie|indischer rupien|indische rupien|indischen rupien|inr|₹")
        .put("Seychellois rupee", "seychellen-rupie|seychellen-rupien|scr|sr|sre")
        .put("Mauritian rupee", "mauritius-rupie|mauritius-rupien|mur")
        .put("Maldivian rufiyaa", "maledivischer rufiyaa|maledivische rufiyaa|maledivischen rufiyaa|mvr|.ރ")
        .put("Sri Lankan rupee", "sri-lanka-rupie|sri-lanka-rupien|lkr|රු|ரூ")
        .put("Indonesian rupiah", "indonesischer rupiah|indonesische rupiah|indonesischen rupiah|rupiah|perak|rp|idr")
        .put("Rupee", "rupie|rs")
        .put("Danish krone", "dänische krone|dänischen krone|dänischer kronen|dänische kronen|dänischen kronen|daenische krone|daenischen krone|daenischer kronen|daenische kronen|daenischen kronen|dkk")
        .put("Norwegian krone", "norwegische krone|norwegischen krone|norwegischer kronen|norwegische kronen|norwegischen kronen|nok")
        .put("Faroese króna", "färöische króna|färöische krone|färöischen krone|färöischer kronen|färöische kronen|färöischen kronen")
        .put("Icelandic króna", "isländische krone|isländischen krone|isländischer kronen|isländische kronen|isländischen kronen|isk")
        .put("Swedish krona", "schwedische krone|schwedischen krone|schwedischer kronen|schwedische kronen|schwedischen kronen|sek")
        .put("Krone", "krone|kronen|kr|-kr")
        .put("Øre", "Øre|oyra|eyrir")
        .put("West African CFA franc", "west african cfa franc|xof|westafrikanische cfa franc|westafrikanische-cfa-franc")
        .put("Central African CFA franc", "central african cfa franc|xaf|zentralafrikanische cfa franc|zentralafrikanische-cfa-franc")
        .put("Comorian franc", "komoren-franc|kmf")
        .put("Congolese franc", "kongo-franc|cdf")
        .put("Burundian franc", "burundi-franc|bif")
        .put("Djiboutian franc", "dschibuti-franc|djf")
        .put("CFP franc", "cfp-franc|xpf")
        .put("Guinean franc", "franc guinéen|franc-guinéen|gnf")
        .put("Swiss franc", "schweizer franken|schweizer-franken|chf|sfr.")
        .put("Rwandan franc", "ruanda-franc|rwf|rf|r₣|frw")
        .put("Belgian franc", "belgischer franken|belgische franken|belgischen franken|bi.|b.fr.|bef")
        .put("Rappen", "rappen|-rappen")
        .put("Franc", "franc|französischer franc|französische franc|französischen franc|französischer franken|französische franken|französischen franken|franken|fr.|fs")
        .put("Centime", "centimes|centime|santim")
        .put("Russian ruble", "russischer rubel|russische rubel|russischen rubel|₽|rub")
        .put("New Belarusian ruble", "neuer weißrussischer rubel|neue weißrussische rubel|neuen weißrussischen rubel|neuem weißrussischen rubel")
        .put("Old Belarusian ruble", "alter weißrussischer rubel|alte weißrussische rubel|alten weißrussischen rubel|altem weißrussischen rubel")
        .put("Transnistrian ruble", "transnistrischer rubel|transnistrische rubel|transnistrischen rubel|prb|р.")
        .put("Belarusian ruble", "weißrussischer rubel|weißrussische rubel|weißrussischen rubel")
        .put("Kopek", "kopek|kopeks")
        .put("Kapyeyka", "kapyeyka")
        .put("Ruble", "rubel|br")
        .put("Algerian dinar", "algerischer dinar|algerische dinar|algerischen dinar|د.ج|dzd")
        .put("Bahraini dinar", "bahrain-dinar|bhd|.د.ب")
        .put("Santeem", "santeem|santeeme")
        .put("Iraqi dinar", "irakischer dinar|irakische dinar|irakischen dinar|iqd|ع.د")
        .put("Jordanian dinar", "jordanischer dinar|jordanische dinar|jordanischen dinar|د.ا|jod")
        .put("Kuwaiti dinar", "kuwait-dinar|kwd|د.ك")
        .put("Libyan dinar", "libyscher dinar|libysche dinar|libyschen dinar|lyd")
        .put("Serbian dinar", "serbischer dinar|serbische dinar|serbischen dinar|din.|rsd|дин.")
        .put("Tunisian dinar", "tunesischer dinar|tunesische dinar|tunesischen dinar|tnd")
        .put("Yugoslav dinar", "jugoslawischer dinar|jugoslawische dinar|jugoslawischen dinar|yun")
        .put("Dinar", "dinar|denar")
        .put("Fils", "fils|fulūs")
        .put("Para", "para|napa")
        .put("Millime", "millime")
        .put("Argentine peso", "argentinischer peso|argentinische peso|argentinischen peso|ars")
        .put("Chilean peso", "chilenischer peso|chilenische peso|chilenischen peso|clp")
        .put("Colombian peso", "kolumbianischer peso|kolumbianische peso|kolumbianischen peso|cop")
        .put("Cuban convertible peso", "kubanischer peso convertible|kubanische peso convertible|kubanischen peso convertible|peso convertible|cuc")
        .put("Cuban peso", "kubanischer peso|kubanische peso|kubanischen peso|cup")
        .put("Dominican peso", "dominican pesos|dominican peso|dop|dominica pesos|dominica peso")
        .put("Mexican peso", "mexikanischer peso|mexikanische peso|mexikanischen peso|mxn")
        .put("Philippine peso", "piso|philippinischer peso|philippinische peso|philippinischen peso|₱|php")
        .put("Uruguayan peso", "uruguayischer peso|uruguayische peso|uruguayischen peso|uyu")
        .put("Peso", "peso")
        .put("Centavo", "centavos|centavo")
        .put("Alderney pound", "alderney pfund|alderney £")
        .put("British pound", "britischer pfund|britische pfund|britischen pfund|british £|gbp|pfund sterling")
        .put("Guernsey pound", "guernsey-pfund|guernsey £|ggp")
        .put("Ascension pound", "ascension-pfund|ascension pound|ascension £")
        .put("Saint Helena pound", "st.-helena-pfund|saint helena £|shp")
        .put("Egyptian pound", "ägyptisches pfund|ägyptische pfund|ägyptischen pfund|ägyptisches £|egp|ج.م")
        .put("Falkland Islands pound", "falkland-pfund|falkland £|fkp|falkland-£")
        .put("Gibraltar pound", "gibraltar-pfund|gibraltar £|gibraltar-£|gip")
        .put("Manx pound", "isle-of-man-pfund|isle-of-man-£|imp")
        .put("Jersey pound", "jersey-pfund|jersey-£|jep")
        .put("Lebanese pound", "libanesisches pfund|libanesische pfund|libanesischen pfund|libanesisches-£|lbp|ل.ل")
        .put("South Georgia and the South Sandwich Islands pound", "süd-georgien & die südlichen sandwichinseln pfund|süd-georgien & die südlichen sandwichinseln £")
        .put("South Sudanese pound", "südsudanesisches pfund|südsudanesische pfund|südsudanesischen pfund|südsudanesisches £|ssp|südsudanesische £")
        .put("Sudanese pound", "sudanesisches pfund|sudanesische pfund|sudanesischen pfund|sudanesisches £|ج.س.|sdg|sudanesische £")
        .put("Syrian pound", "syrisches pfund|syrische pfund|syrischen pfund|syrisches £|ل.س|syp|syrische £")
        .put("Tristan da Cunha pound", "tristan-da-cunha-pfund|tristan-da-cunha-£")
        .put("Pound", "pfund|£")
        .put("Pence", "pence")
        .put("Shilling", "shillings|shilling|shilingi|sh")
        .put("Penny", "pennies|penny")
        .put("United States dollar", "us-dollar|us$|usd|amerikanischer dollar|amerikanische dollar|amerikanischen dollar")
        .put("East Caribbean dollar", "ostkaribischer dollar|ostkaribische dollar|ostkaribischen dollar|ostkaribische $|xcd")
        .put("Australian dollar", "australischer dollar|australische dollar|australischen dollar|australische $|aud")
        .put("Bahamian dollar", "bahama-dollar|bahama-$|bsd")
        .put("Barbadian dollar", "barbados-dollar|barbados-$|bbd")
        .put("Belize dollar", "belize-dollar|belize-$|bzd")
        .put("Bermudian dollar", "bermuda-dollar|bermuda-$|bmd")
        .put("British Virgin Islands dollar", "british virgin islands dollars|british virgin islands dollar|british virgin islands $|bvi$|virgin islands dollars|virgin islands dolalr|virgin islands $|virgin island dollars|virgin island dollar|virgin island $")
        .put("Brunei dollar", "brunei-dollar|brunei $|bnd")
        .put("Sen", "sen")
        .put("Singapore dollar", "singapur-dollar|singapur-$|s$|sgd")
        .put("Canadian dollar", "kanadischer dollar|kanadische dollar|kanadischen dollar|cad|can$|c$")
        .put("Cayman Islands dollar", "kaiman-dollar|kaiman-$|kyd|ci$")
        .put("New Zealand dollar", "neuseeland-dollar|neuseeland-$|nz$|nzd|kiwi")
        .put("Cook Islands dollar", "cookinseln-dollar|cookinseln-$")
        .put("Fijian dollar", "fidschi-dollar|fidschi-$|fjd")
        .put("Guyanese dollar", "guyana-dollar|gyd|gy$")
        .put("Hong Kong dollar", "hongkong-dollar|hong kong $|hk$|hkd|hk dollars|hk dollar|hk $|hongkong$")
        .put("Jamaican dollar", "jamaika-dollar|jamaika-$|j$")
        .put("Kiribati dollar", "kiribati-dollar|kiribati-$")
        .put("Liberian dollar", "liberianischer dollar|liberianische dollar|liberianischen dollar|liberianische $|lrd")
        .put("Micronesian dollar", "mikronesischer dollar|mikronesische dollar|mikronesischen dollar|mikronesische $")
        .put("Namibian dollar", "namibia-dollar|namibia-$|nad|n$")
        .put("Nauruan dollar", "nauru-dollar|nauru-$")
        .put("Niue dollar", "niue-dollar|niue-$")
        .put("Palauan dollar", "palau-dollar|palau-$")
        .put("Pitcairn Islands dollar", "pitcairninseln-dollar|pitcairninseln-$")
        .put("Solomon Islands dollar", "salomonen-dollar|salomonen-$|si$|sbd")
        .put("Surinamese dollar", "suriname-dollar|suriname-$|srd")
        .put("New Taiwan dollar", "neuer taiwan-dollar|neue taiwan-dollar|neuen taiwan-dollar|nt$|twd|ntd")
        .put("Trinidad and Tobago dollar", "trinidad-und-tobago-dollar|trinidad-und-tobago-$|ttd")
        .put("Tuvaluan dollar", "tuvaluischer dollar|tuvaluische dollar|tuvaluischen dollar|tuvaluische $")
        .put("Dollar", "dollar|$")
        .put("Chinese yuan", "yuan|chinesischer yuan|chinesische yuan|chinesischen yuan|renminbi|cny|rmb|￥")
        .put("Fen", "fen")
        .put("Jiao", "jiao")
        .put("Finnish markka", "suomen markka|finnish markka|finsk mark|fim|markkaa|markka|finnische mark|finnischen mark")
        .put("Penni", "penniä|penni")
        .build();

    public static final String CompoundUnitConnectorRegex = "(?<spacer>[^.])";

    public static final ImmutableMap<String, String> CurrencyPrefixList = ImmutableMap.<String, String>builder()
        .put("Dollar", "$")
        .put("United States dollar", "united states $|us$|us $|u.s. $|u.s $")
        .put("East Caribbean dollar", "east caribbean $")
        .put("Australian dollar", "australian $|australia $")
        .put("Bahamian dollar", "bahamian $|bahamia $")
        .put("Barbadian dollar", "barbadian $|barbadin $")
        .put("Belize dollar", "belize $")
        .put("Bermudian dollar", "bermudian $")
        .put("British Virgin Islands dollar", "british virgin islands $|bvi$|virgin islands $|virgin island $|british virgin island $")
        .put("Brunei dollar", "brunei $|b$")
        .put("Sen", "sen")
        .put("Singapore dollar", "singapore $|s$")
        .put("Canadian dollar", "canadian $|can$|c$|c $|canada $")
        .put("Cayman Islands dollar", "cayman islands $|ci$|cayman island $")
        .put("New Zealand dollar", "new zealand $|nz$|nz $")
        .put("Cook Islands dollar", "cook islands $|cook island $")
        .put("Fijian dollar", "fijian $|fiji $")
        .put("Guyanese dollar", "gy$|gy $|g$|g $")
        .put("Hong Kong dollar", "hong kong $|hk$|hkd|hk $")
        .put("Jamaican dollar", "jamaican $|j$|jamaica $")
        .put("Kiribati dollar", "kiribati $")
        .put("Liberian dollar", "liberian $|liberia $")
        .put("Micronesian dollar", "micronesian $")
        .put("Namibian dollar", "namibian $|nad|n$|namibia $")
        .put("Nauruan dollar", "nauruan $")
        .put("Niue dollar", "niue $")
        .put("Palauan dollar", "palauan $")
        .put("Pitcairn Islands dollar", "pitcairn islands $|pitcairn island $")
        .put("Solomon Islands dollar", "solomon islands $|si$|si $|solomon island $")
        .put("Surinamese dollar", "surinamese $|surinam $")
        .put("New Taiwan dollar", "nt$|nt $")
        .put("Trinidad and Tobago dollar", "trinidad and tobago $|trinidad $|trinidadian $")
        .put("Tuvaluan dollar", "tuvaluan $")
        .put("Samoan tālā", "ws$")
        .put("Chinese yuan", "￥")
        .put("Japanese yen", "¥")
        .put("Euro", "€")
        .put("Pound", "£")
        .put("Costa Rican colón", "₡")
        .put("Turkish lira", "₺")
        .build();

    public static final List<String> AmbiguousCurrencyUnitList = Arrays.asList("din.", "kiwi", "kina", "kobo", "lari", "lipa", "napa", "para", "sfr.", "taka", "tala", "toea", "vatu", "yuan", "ang", "ban", "bob", "btn", "byr", "cad", "cop", "cup", "dop", "gip", "jod", "kgs", "lak", "lei", "mga", "mop", "nad", "omr", "pul", "sar", "sbd", "scr", "sdg", "sek", "sen", "sol", "sos", "std", "try", "yer", "yen");

    public static final ImmutableMap<String, String> InformationSuffixList = ImmutableMap.<String, String>builder()
        .put("Bit", "-bit|bit|bits")
        .put("Kilobit", "kilobit|kilobits|kb|kbit")
        .put("Megabit", "megabit|megabits|Mb|Mbit")
        .put("Gigabit", "gigabit|gigabits|Gb|Gbit")
        .put("Terabit", "terabit|terabits|Tb|Tbit")
        .put("Petabit", "petabit|petabits|Pb|Pbit")
        .put("Byte", "byte|bytes")
        .put("Kilobyte", "kilobyte|kB|kilobytes|kilo byte|kilo bytes|kByte")
        .put("Megabyte", "megabyte|mB|megabytes|mega byte|mega bytes|MByte")
        .put("Gigabyte", "gigabyte|gB|gigabytes|giga byte|giga bytes|GByte")
        .put("Terabyte", "terabyte|tB|terabytes|tera byte|tera bytes|TByte")
        .put("Petabyte", "petabyte|pB|petabytes|peta byte|peta bytes|PByte")
        .build();

    public static final List<String> AmbiguousDimensionUnitList = Arrays.asList("barrel", "grain", "gran", "grän", "korn", "pfund", "stone", "yard", "cord", "dram", "fuß", "gill", "knoten", "peck", "cup", "fps", "pts", "in", "\"");

    public static final String BuildPrefix = "(?<=(\\s|^))";

    public static final String BuildSuffix = "(?=(\\s|\\W|$))";

    public static final ImmutableMap<String, String> LengthSuffixList = ImmutableMap.<String, String>builder()
        .put("Kilometer", "km|kilometer|kilometern")
        .put("Hectometer", "hm|hektometer|hektometern")
        .put("Decameter", "dam|dekameter|dekametern")
        .put("Meter", "m|meter|metern")
        .put("Decimeter", "dm|dezimeter|dezimetern")
        .put("Centimeter", "cm|zentimeter|centimeter|zentimetern|centimetern")
        .put("Millimeter", "mm|millimeter|millimetern")
        .put("Micrometer", "μm|mikrometer|mikrometern")
        .put("Nanometer", "nm|nanometer|nanometern")
        .put("Picometer", "pm|pikometer|picometer|pikometern|picometern")
        .put("Mile", "meile|meilen")
        .put("Yard", "yard|yards")
        .put("Inch", "zoll|inch|in|\"")
        .put("Foot", "fuß|ft")
        .put("Light year", "lichtjahr|lichtjahre|lichtjahren")
        .put("Pt", "pt|pts")
        .build();

    public static final List<String> AmbiguousLengthUnitList = Arrays.asList("m", "yard", "yards", "pm", "pt", "pts");

    public static final ImmutableMap<String, String> SpeedSuffixList = ImmutableMap.<String, String>builder()
        .put("Meter per second", "meter/sekunde|m/s|meter pro sekunde|metern pro sekunde")
        .put("Kilometer per hour", "km/h|kilometer/stunde|kilometer pro stunde|kilometern pro stunde")
        .put("Kilometer per minute", "km/min|kilometer pro minute|kilometern pro minute")
        .put("Kilometer per second", "km/s|kilometer pro sekunde|kilometern pro sekunde")
        .put("Mile per hour", "mph|mi/h|meilen pro stunde|meilen/stunde|meile pro stunde")
        .put("Knot", "kt|knoten|kn")
        .put("Foot per second", "ft/s|fuß/sekunde|fuß pro sekunde|fps")
        .put("Foot per minute", "ft/min|fuß/minute|fuß pro minute")
        .put("Yard per minute", "yard pro minute|yard/minute|yard/min")
        .put("Yard per second", "yard pro sekunde|yard/sekunde|yard/s")
        .build();

    public static final ImmutableMap<String, String> TemperatureSuffixList = ImmutableMap.<String, String>builder()
        .put("F", "grad fahrenheit|°fahrenheit|°f|fahrenheit")
        .put("K", "k|K|kelvin")
        .put("R", "rankine|°r")
        .put("D", "delisle|°de")
        .put("C", "grad celsius|°celsius|°c")
        .put("Degree", "grad|°")
        .build();

    public static final List<String> AmbiguousTemperatureUnitList = Arrays.asList("c", "f", "k");

    public static final ImmutableMap<String, String> VolumeSuffixList = ImmutableMap.<String, String>builder()
        .put("Cubic meter", "m3|kubikmeter|m³")
        .put("Cubic centimeter", "kubikzentimeter|cm³")
        .put("Cubic millimiter", "kubikmillimeter|mm³")
        .put("Hectoliter", "hektoliter")
        .put("Decaliter", "dekaliter")
        .put("Liter", "l|liter")
        .put("Deciliter", "dl|deziliter")
        .put("Centiliter", "cl|zentiliter")
        .put("Milliliter", "ml|mls|milliliter")
        .put("Cubic yard", "kubikyard")
        .put("Cubic inch", "kubikzoll")
        .put("Cubic foot", "kubikfuß")
        .put("Cubic mile", "kubikmeile")
        .put("Fluid ounce", "fl oz|flüssigunze|fluessigunze")
        .put("Teaspoon", "teelöffel|teeloeffel")
        .put("Tablespoon", "esslöffel|essloeffel")
        .put("Pint", "pinte")
        .put("Volume unit", "fluid dram|Fluid drachm|Flüssigdrachme|Gill|Quart|Minim|Barrel|Cord|Peck|Beck|Scheffel|Hogshead|Oxhoft")
        .build();

    public static final List<String> AmbiguousVolumeUnitList = Arrays.asList("l", "unze", "oz", "cup", "peck", "cord", "gill");

    public static final ImmutableMap<String, String> WeightSuffixList = ImmutableMap.<String, String>builder()
        .put("Kilogram", "kg|kilogramm|kilo")
        .put("Gram", "g|gramm")
        .put("Milligram", "mg|milligramm")
        .put("Barrel", "barrel")
        .put("Gallon", "gallone|gallonen")
        .put("Metric ton", "metrische tonne|metrische tonnen")
        .put("Ton", "tonne|tonnen")
        .put("Pound", "pfund|lb")
        .put("Ounce", "unze|unzen|oz|ounces")
        .put("Weight unit", "pennyweight|grain|british long ton|US short hundredweight|stone|dram")
        .build();

    public static final List<String> AmbiguousWeightUnitList = Arrays.asList("g", "oz", "stone", "dram");

    public static final List<String> AmbiguousAgeUnitList = Arrays.asList("jahren", "jahre", "monaten", "monate", "wochen", "woche", "tagen", "tage");
}
