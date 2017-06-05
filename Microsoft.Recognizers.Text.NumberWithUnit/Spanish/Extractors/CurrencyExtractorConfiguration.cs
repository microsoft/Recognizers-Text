using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class CurrencyExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public CurrencyExtractorConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public CurrencyExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => CurrencySuffixList;

        public override ImmutableDictionary<string, string> PrefixList => CurrencyPrefixList;

        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_CURRENCY;

        public static readonly ImmutableDictionary<string, string> CurrencySuffixList = new Dictionary<string, string>
        {
            //Reference Source: https://es.wikipedia.org/wiki/Anexo:Monedas_circulantes
            //GeneralsUnits
            { "Dólar", "dólar|dólares" },
            { "Peso", "peso|pesos" },
            { "Rublo", "rublo|rublos"},
            { "Libra", "libra|libras" },
            { "Florín", "florín|florines" },
            { "Dinar", "dinar|dinares" },
            { "Franco", "franco|francos" },
            { "Rupia", "rupia|rupias" },
            { "Escudo", "escudo|escudos" },
            { "Chelín", "chelín|chelines" },
            { "Lira", "lira|liras" },
            { "Centavo", "centavo|centavos" },
            { "Céntimo", "céntimo|céntimos" },
            { "Centésimo", "centésimo|centésimos" },
            { "Penique", "penique|peniques" },

            //Euro
            { "Euro", "euro|euros|€|eur" },
            { "Céntimo de Euro", "céntimo de euro|céntimos de euros" },
            //Dólar del Caribe Oriental
            { "Dólar del Caribe Oriental", "dólar del Caribe Oriental|dólares del Caribe Oriental|ec$|xcd" },
            { "Centavo del Caribe Oriental", "centavo del Caribe Oriental|centavos del Caribe Oriental" },
            //Franco CFA de África Occidental
            { "Franco CFA de África Occidental", "franco CFA de África Occidental|francos CFA de África Occidental|fcfa|xof" },
            { "Céntimo de CFA de África Occidental", "céntimo de CFA de África Occidental|céntimos de CFA de África Occidental" },
            //Franco CFA de África Central
            { "Franco CFA de África Central", "franco CFA de África Central|francos CFA de África Central|xaf" },
            { "Céntimo de CFA de África Central", "céntimo de CFA de África Central|céntimos de CFA de África Central" },
            //Abjasia
            {"Apsar", "apsar|apsares"},
            //Afganistán
            {"Afgani afgano", "afgani afgano|؋|afn|afganis|afgani"},
            {"Pul", "pul|puls"},
            //Albania
            {"Lek albanés", "lek|lekë|lekes|lek albanés"},
            {"Qindarka", "qindarka|qindarkë|qindarkas"},
            //Alderney -> Libra británica | Libra de Guernsey
            //Alemania -> Euro
            //Andorra -> Euro
            //Angola
            {"Kwanza angoleño", "kwanza angoleño|kwanzas angoleños|kwanza angoleños|kwanzas angoleño|kwanzas|aoa|kz" },
            {"Cêntimo angoleño", "cêntimo angoleño|cêntimo|cêntimos" },
            //Anguila -> Dólar del Caribe Oriental
            //Antigua y Barbuda -> Dólar del Caribe Oriental
            //Antillas Neerlandesas
            { "Florín antillano neerlandés", "florín antillano neerlandés|florínes antillano neerlandés|ƒ antillano neerlandés|ang|naƒ" },
            { "Cent antillano neerlandés", "cent|centen" },
            //Arabia Saudita
            { "Riyal saudí", "riyal saudí|riyales saudí|sar" },
            { "Halalá saudí", "halalá saudí|hallalah" },
            //Argelia
            { "Dinar argelino", "dinar argelino|dinares argelinos|dzd" },
            { "Céntimo argelino", "centimo argelino|centimos argelinos|" },
            //Argentina
            { "Peso argentino", "peso argentino|pesos argentinos|peso|pesos|ar$|ars" },
            { "Centavo argentino", "centavo argentino|centavos argentinos|centavo|ctvo.|ctvos." },
            //Armenia
            { "Dram armenio", "dram armenio|dram armenios|dram|դր." },
            { "Luma armenio", "luma armenio|luma armenios" },
            //Aruba
            { "Florín arubeño", "florín arubeño|florines arubeños|ƒ arubeños|aƒ|awg" },
            { "Yotin arubeño", "yotin arubeño|yotines arubeños" },
            //Australia
            { "Dólar australiano", "dólar australiano|dólares australianos|a$|aud" },
            { "Centavo australiano", "centavo australiano|centavos australianos" },
            //Austria -> Euro
            //Azerbaiyán
            {"Manat azerí", "manat azerí|man|azn" },
            {"Qəpik azerí", "qəpik azerí|qəpik" },
            //Bahamas
            { "Dólar bahameño", "dólar bahameño|dólares bahameños|b$|bsd" },
            { "Centavo bahameño", "centavo bahameño|centavos bahameños" },
            //Baréin
            { "Dinar bahreiní", "dinar bahreiní|dinares bahreinies|bhd" },
            { "Fil bahreiní", "fil bahreiní|fils bahreinies" },
            //Bangladés
            { "Taka bangladeshí", "taka bangladeshí|takas bangladeshí|bdt" },
            { "Poisha bangladeshí", "poisha bangladeshí|poishas bangladeshí" },
            //Barbados
            { "Dólar de Barbados", "dólar de barbados|dólares de barbados|bbd" },
            { "Centavo de Barbados", "centavo de barbados|centavos de barbados" },
            //Bélgica -> Euro
            //Belice
            { "Dólar beliceño", "dólar beliceño|dólares beliceños|bz$|bzd" },
            { "Centavo beliceño", "centavo beliceño|centavos beliceños" },
            //Benín -> Franco CFA de África Occidental
            //Bermudas
            { "Dólar bermudeño", "dólar bermudeño|dólares bermudeños|bd$|bmd" },
            { "Centavo bermudeño", "centavo bermudeño|centavos bermudeños" },
            //Bielorrusia
            { "Rublo bielorruso", "rublo bielorruso|rublos bielorrusos|br|byr" },
            { "Kópek bielorruso", "kópek bielorruso|kópeks bielorrusos|kap" },
            //Birmania
            { "Kyat birmano", "kyat birmano|kyats birmanos|mmk" },
            { "Pya birmano", "pya birmano|pyas birmanos" },
            //Bolivia
            { "Boliviano", "boliviano|bolivianos|bob|bs" },
            { "Centésimo Boliviano", "centésimo boliviano|centésimos bolivianos" },
            //Bosnia y Herzegovina
            { "Marco bosnioherzegovino", "marco convertible|marco bosnioherzegovino|marcos convertibles|marcos bosnioherzegovinos|bam" },
            { "Feningas bosnioherzegovino", "feninga convertible|feninga bosnioherzegovina|feningas convertibles" },
            //Botsuana
            { "Pula", "pula|bwp" },
            { "Thebe", "thebe" },
            //Brasil
            { "Real brasileño", "real brasileño|reales brasileños|r$|brl" },
            { "Centavo brasileño", "centavo brasileño|centavos brasileños" },
            //Brunéi
            { "Dólar de Brunéi", "dólar de brunei|dólares de brunéi|bnd" },
            { "Sen de Brunéi", "sen|sen de brunéi" },
            //Bulgaria
            { "Lev búlgaro", "lev búlgaro|leva búlgaros|lv|bgn" },
            { "Stotinki búlgaro", "stotinka búlgaro|stotinki búlgaros" },
            //Burkina Faso -> Franco CFA de África Occidental
            //Burundi
            { "Franco de Burundi", "franco de burundi|francos de burundi|fbu|fib" },
            { "Céntimo Burundi", "céntimo burundi|céntimos burundies" },
            //Bután
            { "Ngultrum butanés", "ngultrum butanés|ngultrum butaneses|btn" },
            { "Chetrum  butanés", "chetrum butanés|chetrum butaneses" },
            //Cabo Verde
            { "Escudo caboverdiano", "escudo caboverdiano|escudos caboverdianos|cve" },
            //Camboya
            { "Riel camboyano", "riel camboyano|rieles camboyanos|khr" },
            //Camerún -> Franco CFA de África Central
            //Canadá
            { "Dólar canadiense", "dólar canadiense|dólares canadienses|c$|cad" },
            { "Centavo canadiense", "centavo canadiense|centavos canadienses" },
            //Chad -> Franco CFA de África Central
            //Chile
            { "Peso chileno", "peso chileno|pesos chilenos|cpl" },
            //China
            { "Yuan chino", "yuan chino|yuanes chinos|yuan|yuanes|renminbi|rmb|cny|¥" },
            //Chipre -> Euro
            //Colombia
            { "Peso colombiano", "peso colombiano|pesos colombianos|cop|col$" },
            { "Centavo colombiano", "centavo colombiano|centavos colombianos" },
            //Comoras
            { "Franco comorano", "franco comorano|francos comoranos|kmf|₣" },
            //Congo, República del -> Franco CFA de África Central
            //Congo, República Democrática del
            { "Franco congoleño", "franco congoleño|francos congoleños|cdf" },
            { "Céntimo congoleño", "céntimo congoleño|céntimos congoleños" },
            //Corea del Norte
            { "Won norcoreano", "won norcoreano|wŏn norcoreano|wŏn norcoreanos|kpw" },
            { "Chon norcoreano", "chon norcoreano|chŏn norcoreano|chŏn norcoreanos|chon norcoreanos" },
            //Corea del Sur
            { "Won surcoreano", "wŏn surcoreano|won surcoreano|wŏnes surcoreanos|wones surcoreanos|krw" },
            { "Chon surcoreano", "chon surcoreano|chŏn surcoreano|chŏn surcoreanos|chon surcoreanos" },
            //Costa de Marfil -> Franco CFA de África Occidental
            //Costa Rica
            { "Colón costarricense", "colón costarricense|colones costarricenses|crc" },
            //Croacia
            { "Kuna croata", "kuna croata|kuna croatas|hrk" },
            { "Lipa croata", "lipa croata|lipa croatas" },
            //Cuba
            { "Peso cubano", "peso cubano|pesos cubanos|cup" },
            { "Peso cubano convertible", "peso cubano convertible|pesos cubanos convertible|cuc" },
            //Dinamarca
            { "Corona danesa", "corona danesa|coronas danesas|dkk" },
            //Dominica -> Dólar del Caribe Oriental
            //Ecuador -> Dólar estadounidense
            //Egipto
            { "Libra egipcia", "libra egipcia|libras egipcias|egp|le" },
            { "Piastra egipcia", "piastra egipcia|piastras egipcias" },
            //El Salvador
            { "Colón salvadoreño", "colón salvadoreño|colones salvadoreños|svc" },
            //Emiratos Árabes Unidos
            { "Dirham de los Emiratos Árabes Unidos", "dirham|dirhams|dirham de los Emiratos Árabes Unidos|aed|dhs" },
            //Eritrea
            { "Nakfa", "nakfa|nfk|ern" },
            { "Céntimo de Nakfa", "céntimo de nakfa|céntimos de nakfa" },
            //Eslovaquia -> Euro
            //Eslovenia -> Euro
            //España -> Euro
            { "Peseta", "peseta|pesetas|pts.|ptas.|esp" },
            //Estados Unidos
            { "Dólar estadounidense", "dólar estadounidense|dólares estadounidenses|usd|u$d|us$" },
            //Estonia
            { "Corona estonia", "corona estonia|coronas estonias|eek" },
            { "Senti estonia", "senti estonia|senti estonias" },
            //Etiopía
            { "Birr etíope", "birr etíope|birr etíopes|br|etb" },
            { "Santim etíope", "santim etíope|santim etíopes" },
            //Filipinas
            { "Peso filipino", "peso filipino|pesos filipinos|php" },
            //Finlandia -> Euro
            { "Marco finlandés", "marco finlandés|marcos finlandeses" },
            //Fiyi
            { "Dólar fiyiano", "dólar fiyiano|dólares fiyianos|fj$|fjd" },
            { "Centavo fiyiano", "centavo fiyiano|centavos fiyianos" },
            //Francia -> Euro
            //Gabón -> Franco CFA de África Central
            //Gambia
            { "Dalasi", "dalasi|gmd" },
            { "Bututs", "butut|bututs" },
            //Georgia
            { "Lari georgiano", "lari georgiano|lari georgianos|gel" },
            { "Tetri georgiano", "tetri georgiano|tetri georgianos" },
            //Ghana
            { "Cedi", "cedi|ghs|gh₵" },
            { "Pesewa", "pesewa" },
            //Gibraltar
            { "Libra gibraltareña", "libra gibraltareña|libras gibraltareñas|gip" },
            { "Penique gibraltareña", "penique gibraltareña|peniques gibraltareñas" },
            //Granada -> Dólar del Caribe Oriental
            //Grecia -> Euro
            //Guatemala
            { "Quetzal guatemalteco", "quetzal guatemalteco|quetzales guatemaltecos|quetzal|quetzales|gtq" },
            { "Centavo guatemalteco", "centavo guatemalteco|centavos guatemaltecos" },
            //Guernsey
            { "Libra de Guernsey", "libra de Guernsey|libras de Guernsey|ggp" },
            { "Penique de Guernsey", "penique de Guernsey|peniques de Guernsey" },
            //Guinea
            { "Franco guineano", "franco guineano|francos guineanos|gnf|fg" },
            { "Céntimo guineano", "céntimo guineano|céntimos guineanos" },
            //Guinea-Bisáu -> Franco CFA de África Occidental
            //Guinea Ecuatorial -> Franco CFA de África Central
            //Guyana
            { "Dólar guyanés", "dólar guyanés|dólares guyaneses|gyd|gy" },
            //Haití
            { "Gourde haitiano", "gourde haitiano|gourde haitianos|htg" },
            { "Céntimo haitiano", "céntimo haitiano|céntimos haitianos" },
            //Honduras
            { "Lempira hondureño", "lempira hondureño|lempira hondureños|hnl" },
            { "Centavo hondureño", "centavo hondureño|centavos hondureño" },
            //Hong Kong
            { "Dólar de Hong Kong", "dólar de hong kong|dólares de hong kong|hk$|hkd" },
            //Hungría
            { "Forinto húngaro", "forinto húngaro|forinto húngaros|huf" },
            //India
            { "Rupia india", "rupia india|rupias indias|inr" },
            { "Paisa india", "paisa india|paise indias" },
            //Indonesia
            { "Rupia indonesia", "rupia indonesia|rupias indonesias|idr" },
            { "Sen indonesia", "sen indonesia|sen indonesias" },
            //Irán
            { "Rial iraní", "rial iraní|rial iranies|irr" },
            //Irak
            { "Dinar iraquí", "dinar iraquí|dinares iraquies|iqd" },
            { "Fil iraquí", "fil iraquí|fils iraquies" },
            //Irlanda -> Euro
            //Isla Ascensión -> Libra se Santa Elena
            //Isla de Man
            { "Libra manesa", "libra manesa|libras manesas|imp" },
            { "Penique manes", "penique manes|peniques maneses" },
            //Islandia
            { "Corona islandesa", "corona islandesa|coronas islandesas|isk|íkr" },
            { "Aurar islandes", "aurar islandes|aurar islandeses" },
            //Islas Caimán
            { "Dólar de las Islas Caimán", "dólar de las Islas Caimán|dólares de las Islas Caimán|ci$|kyd" },
            //Islas Cocos -> Dólar australiano
            //Islas Cook
            { "Dólar de las Islas Cook", "dólar de las Islas Cook|dólares de las Islas Cook" },
            //Islas Feroe
            { "Corona feroesa", "corona feroesa|coronas feroesas|fkr" },
            //Islas Georgias del Sur y Sandwich del Sur
            //Islas Malvinas
            { "Libra malvinense", "libra malvinense|libras malvinenses|fk£|fkp" },
            //Islas Marianas del Norte -> Dólar estadounidense
            //Islas Marshall -> Dólar estadounidense
            //Islas Pitcairn -> Dólar neozelandés
            //Islas Salomón
            { "Dólar de las Islas Salomón", "dólar de las Islas Salomón|dólares de las Islas Salomón|sbd" },
            //Islas Turcas y Caicos -> Dólar estadounidense
            //Islas Vírgenes Británicas -> Dólar estadounidense
            //Israel
            { "Nuevo shéquel", "nuevo shéquel|nuevos shéquel|ils" },
            { "Agorot", "agorot" },
            //Italia -> Euro
            //Jamaica
            { "Dólar jamaiquino", "dólar jamaiquino|dólares jamaiquinos|j$|ja$|jmd" },
            //Japón
            { "Yen", "yen|yenes|jpy" },
            //Jersey
            { "Libra de Jersey", "libra de Jersey|libras de Jersey|jep" },
            //Jordania
            { "Dinar jordano", "dinar jordano|dinares jordanos|jd|jod" },
            { "Piastra jordano", "piastra jordano|piastras jordanos" },
            //Kazajistán
            { "Tenge kazajo", "tenge|tenge kazajo|kzt" },
            //Kenia
            { "Chelín keniano", "chelín keniano|chelines kenianos|ksh|kes" },
            //Kirguistán
            { "Som kirguís", "som kirguís|kgs" },
            { "Tyiyn", "tyiyn" },
            //Kiribati
            { "Dólar de Kiribati", "dólar de Kiribati|dólares de Kiribati" },
            //Kosovo -> Euro
            //Kuwait
            { "Dinar kuwaití", "dinar kuwaití|dinares kuwaití" },
            //Laos
            { "Kip laosiano", "kip|kip laosiano|kip laosianos|lak" },
            { "Att laosiano", "att|att laosiano|att laosianos" },
            //Lesoto
            { "Loti", "loti|maloti|lsl" },
            { "Sente", "sente|lisente" },
            //Letonia -> Euro
            //Líbano
            { "Libra libanesa", "libra libanesa|libras libanesas|lbp" },
            //Liberia
            { "Dólar liberiano", "dólar liberiano|dólares liberianos|l$|lrd" },
            //Libia
            { "Dinar libio", "dinar libio|dinares libios|ld|lyd" },
            { "Dirham libio", "dirham libio|dirhams libios" },
            //Liechtenstein -> Franco suizo
            //Lituania -> Euro
            {"Litas lituana", "litas lituana|litai lituanas|ltl" },
            //Luxemburgo -> Euro
            //Macao
            { "Pataca macaense", "pataca macaense|patacas macaenses|mop$|mop" },
            { "Avo macaense", "avo macaense|avos macaenses" },
            { "Ho macaense", "ho macaense|ho macaenses" },
            //Macedonia, República de
            { "Denar macedonio", "denar macedonio|denare macedonios|den|mkd" },
            { "Deni macedonio", "deni macedonio|deni macedonios" },
            //Madagascar
            { "Ariary malgache", "ariary malgache|ariary malgaches|mga" },
            { "Iraimbilanja malgache", "iraimbilanja malgache|iraimbilanja malgaches" },
            //Malasia
            { "Ringgit malayo", "ringgit malayo|ringgit malayos|rm|myr" },
            { "Sen malayo", "sen malayo|sen malayos" },
            //Malaui
            { "Kwacha malauí", "kwacha malauí|mk|mwk" },
            { "Támbala malauí", "támbala malauí" },
            //Maldivas
            { "Rupia de Maldivas", "rupia de Maldivas|rupias de Maldivas|mvr" },
            //Malí -> Franco CFA de África Occidental
            //Malta -> Euro
            //Marruecos
            { "Dirham marroquí", "dirham marroquí|dirhams marroquies|mad" },
            //Mauricio
            { "Rupia de Mauricio", "rupia de Mauricio|rupias de Mauricio|mur" },
            //Mauritania
            { "Uguiya", "uguiya|uguiyas|mro" },
            { "Jum", "jum|jums" },
            //México
            { "Peso mexicano", "peso mexicano|pesos mexicanos|mxn" },
            { "Centavo mexicano", "centavo mexicano|centavos mexicanos" },
            //Micronesia, Estados Federados de -> Dólar estadounidense
            //Moldavia
            { "Leu moldavo", "leu moldavo|lei moldavos|mdl" },
            { "Ban moldavo", "ban moldavo|bani moldavos" },
            //Mónaco -> Euro
            //Mongolia
            { "Tugrik mongol", "tugrik mongol|tugrik|tugrik mongoles|tug|mnt" },
            //Montenegro -> Euro
            //Montserrat -> Dólar del Caribe Oriental
            //Mozambique
            { "Metical mozambiqueño", "metical|metical mozambiqueño|meticales|meticales mozambiqueños|mtn|mzn" },
            //Nagorno Karabaj
            { "Dram de Nagorno Karabaj", "dram de Nagorno Karabaj|drams de Nagorno Karabaj|" },
            { "Luma de Nagorno Karabaj", "luma de Nagorno Karabaj" },
            //Namibia
            { "Dólar namibio", "dólar namibio|dólares namibios|n$|nad" },
            { "Centavo namibio", "centavo namibio|centavos namibios" },
            //Nauru -> Dólar australiano
            //Nepal
            { "Rupia nepalí", "rupia nepalí|rupias nepalies|npr" },
            { "Paisa nepalí", "paisa nepalí|paisas nepalies" },
            //Nicaragua
            { "Córdoba nicaragüense", "córdoba nicaragüense|córdobas nicaragüenses|c$|nio" },
            { "Centavo nicaragüense", "centavo nicaragüense|centavos nicaragüenses" },
            //Níger -> Franco CFA de África Occidental
            //Nigeria
            { "Naira", "naira|ngn" },
            { "Kobo", "kobo" },
            //Niue -> Dólar neozelandés
            //Noruega
            { "Corona noruega", "corona noruega|coronas noruegas|nok" },
            //Nueva Caledonia
            { "Franco CFP", "franco cfp|francos cfp|xpf" },
            //Nueva Zelanda
            { "Dólar neozelandés", "dólar neozelandés|dólares neozelandeses|dólar de Nueva Zelanda|dólares de Nueva Zelanda|nz$|nzd" },
            { "Centavo neozelandés", "centavo neozelandés|centavo de Nueva Zelanda|centavos de Nueva Zelanda|centavos neozelandeses" },
            //Omán
            { "Rial omaní", "rial omaní|riales omanies|omr" },
            { "Baisa omaní", "baisa omaní|baisa omanies" },
            //Osetia del Sur -> Rublo ruso
            //Países Bajos -> Euro
            { "Florín neerlandés", "florín neerlandés|florines neerlandeses|nlg" },
            //Pakistán
            { "Rupia pakistaní", "rupia pakistaní|rupias pakistanies|pkr" },
            { "Paisa pakistaní", "paisa pakistaní|paisas pakistanies" },
            //Palaos -> Dólar estadounidense
            //Palestina -> Nuevo shéquel | Dinar jordano
            //Panamá
            { "Balboa panameño", "balboa panameño|balboa panameños|pab" },
            { "Centésimo panameño", "centésimo panameño|centésimos panameños" },
            //Papúa Nueva Guinea
            { "Kina", "kina|pkg|pgk" },
            { "Toea", "toea" },
            //Paraguay
            { "Guaraní", "guaraní|guaranies|gs|pyg" },
            //Perú
            { "Sol", "sol|soles|nuevo sol|pen|s//." },
            { "Céntimo de sol", "céntimo de sol|céntimos de sol" },
            //Polinesia Francesa -> Franco CFP
            //Polonia
            { "Złoty", "złoty|esloti|eslotis|zł|pln" },
            { "Groszy", "groszy" },
            //Portugal -> Euro
            //Qatar
            { "Riyal qatarí", "riyal qatarí|riyal qataries|qr|qar" },
            { "Dirham qatarí", "dirham qatarí|dirhams qataries" },
            //Reino Unido
            { "Libra esterlina", "libra esterlina|libras esterlinas|gbp" },
            //República Centroafricana -> Franco CFA de África Central
            //República Checa
            { "Corona checa", "corona checa|coronas checas|kc|czk" },
            //República Dominicana
            { "Peso dominicano", "peso dominicano|pesos dominicanos|rd$|dop" },
            { "Centavo dominicano", "centavo dominicano|centavos dominicanos" },
            //República Turca del Norte de Chipre ->Lira turca
            //Ruanda
            { "Franco ruandés", "franco ruandés|francos ruandeses|rf|rwf" },
            { "Céntimo ruandés", "céntimo ruandés|céntimos ruandeses" },
            //Rumania
            { "Leu rumano", "leu rumano|lei rumanos|ron" },
            { "Ban rumano", "ban rumano|bani rumanos" },
            //Rusia
            { "Rublo ruso", "rublo ruso|rublos rusos|rub" },
            { "Kopek ruso", "kopek ruso|kopeks rusos" },
            //Sahara Occidental -> Dirham marroquí | Dinar argelino
            //Samoa
            { "Tala", "tala|tālā|ws$|sat|wst" },
            { "Sene", "sene" },
            //San Cristóbal y Nieves -> Dólar del Caribe Oriental
            //San Marino -> Euro
            //Santa Elena, Ascensión y Tristán de Acuña
            { "Libra de Santa Helena", "libra de Santa Helena|libras de Santa Helena|shp" },
            { "Penique de Santa Helena", "penique de Santa Helena|peniques de Santa Helena" },
            //Santa Lucía -> Dólar del Caribe Oriental
            //Santo Tomé y Príncipe
            { "Dobra", "dobra|db|std" },
            //San Vicente y las Granadinas -> Dólar del Caribe Oriental
            //Senegal -> Franco CFA de África Occidental
            //Serbia
            { "Dinar serbio", "dinar serbio|dinares serbios|rsd" },
            { "Para serbio", "para serbio|para serbios" },
            //Seychelles
            { "Rupia de Seychelles", "rupia de Seychelles|rupias de Seychelles|scr" },
            { "Centavo de Seychelles", "centavo de Seychelles|centavos de Seychelles" },
            //Sierra Leona
            { "Leone", "leone|le|sll" },
            //Singapur
            { "Dólar de Singapur", "dólar de singapur|dólares de singapur|sgb" },
            { "Centavo de Singapur", "centavo de Singapur|centavos de Singapur" },
            //Siria
            { "Libra siria", "libra siria|libras sirias|s£|syp" },
            { "Piastra siria", "piastra siria|piastras sirias" },
            //Somalia
            { "Chelín somalí", "chelín somalí|chelines somalies|sos" },
            { "Centavo somaplí", "centavo somaplí|centavos somalies" },
            //Somalilandia
            { "Chelín somalilandés", "chelín somalilandés|chelines somalilandeses" },
            { "Centavo somalilandés", "centavo somalilandés|centavos somalilandeses" },
            //Sri Lanka
            { "Rupia de Sri Lanka", "rupia de Sri Lanka|rupias de Sri Lanka|lkr" },
            { "Céntimo de Sri Lanka", "céntimo de Sri Lanka|céntimos de Sri Lanka" },
            //Suazilandia
            { "Lilangeni", "lilangeni|emalangeni|szl" },
            //Sudáfrica
            { "Rand sudafricano", "rand|rand sudafricano|zar" },
            //Sudán
            { "Libra sudanesa", "libra sudanesa|libras sudanesas|sdg" },
            { "Piastra sudanesa", "piastra sudanesa|piastras sudanesas" },
            //Sudán del Sur
            { "Libra sursudanesa", "libra sursudanesa|libras sursudanesa|ssp" },
            { "Piastra sursudanesa", "piastra sursudanesa|piastras sursudanesas" },
            //Suecia
            { "Corona sueca", "corona sueca|coronas suecas|sek" },
            //Suiza
            { "Franco suizo", "franco suizo|francos suizos|sfr|chf" },
            { "Rappen suizo", "rappen suizo|rappens suizos" },
            //Surinam
            { "Dólar surinamés", "óolar surinamés|dólares surinameses|srd" },
            { "Centavo surinamés", "centavo surinamés|centavos surinamés" },
            //Tailandia
            { "Baht tailandés", "baht tailandés|baht tailandeses|thb" },
            { "Satang tailandés", "satang tailandés|satang tailandeses" },
            //Taiwán
            { "Nuevo dólar taiwanés", "nuevo dólar taiwanés|dólar taiwanés|dólares taiwaneses|twd" },
            { "Centavo taiwanés", "centavo taiwanés|centavos taiwaneses" },
            //Tanzania
            { "Chelín tanzano", "chelín tanzano|chelines tanzanos|tzs" },
            { "Centavo tanzano", "centavo tanzano|centavos tanzanos" },
            //Tayikistán
            { "Somoni tayiko", "somoni tayiko|somoni|tjs" },
            { "Diram", "diram|dirams" },
            //Territorio Británico del Océano Índico -> Dólar estadounidense
            //Timor Oriental -> Dólar estadounidense
            //Togo -> Franco CFA de África Occidental
            //Tonga
            { "Paʻanga", "dólar tongano|dólares tonganos|paʻanga|pa'anga|top" },
            { "Seniti", "seniti" },
            //Transnistria
            { "Rublo de Transnistria", "rublo de Transnistria|rublos de Transnistria" },
            { "Kopek de Transnistria", "kopek de Transnistria|kopeks de Transnistria" },
            //Trinidad y Tobago
            { "Dólar trinitense", "dólar trinitense|dólares trinitenses|ttd" },
            { "Centavo trinitense", "centavo trinitense|centavos trinitenses" },
            //Tristán de Acuña -> Libra de Santa Helena
            //Túnez
            { "Dinar tunecino", "dinar tunecino|dinares tunecinos|tnd" },
            { "Millime tunecino", "millime tunecino|millimes tunecinos" },
            //Turquía
            { "Lira turca", "lira turca|liras turcas|try" },
            { "Kuruş turca", "kuruş turca|kuruş turcas" },
            //Turkmenistán
            { "Manat turkmeno", "manat turkmeno|anat turkmenos|tmt" },
            { "Tennesi turkmeno", "tennesi turkmeno|tenge turkmeno" },
            //Tuvalu
            { "Dólar tuvaluano", "dólar tuvaluano|dólares tuvaluanos" },
            { "Centavo tuvaluano", "centavo tuvaluano|centavos tuvaluanos" },
            //Ucrania
            { "Grivna", "grivna|grivnas|uah" },
            { "Kopiyka", "kopiyka|kópeks" },
            //Uganda
            { "Chelín ugandés", "chelín ugandés|chelines ugandeses|ugx" },
            { "Centavo ugandés", "centavo ugandés|centavos ugandeses" },
            //Uruguay
            { "Peso uruguayo", "peso uruguayo|pesos uruguayos|uyu" },
            { "Centésimo uruguayo", "centésimo uruguayo|centésimos uruguayos" },
            //Uzbekistán
            { "Som uzbeko", "som uzbeko|som uzbekos|uzs" },
            { "Tiyin uzbeko", "tiyin uzbeko|tiyin uzbekos" },
            //Vanuatu
            { "Vatu", "vatu|vuv" },
            //Vaticano, Ciudad del -> Euro
            //Venezuela
            { "Bolívar fuerte", "bolívar fuerte|bolívar|bolívares|vef" },
            { "Céntimo de bolívar", "céntimo de bolívar|céntimos de bolívar" },
            //Vietnam
            { "Đồng vietnamita", "Đồng vietnamita|dong vietnamita|dong vietnamitas|vnd" },
            { "Hào vietnamita", "Hào vietnamita|hao vietnamita|hao vietnamitas" },
            //Wallis y Futuna -> Franco CFP
            //Yemen
            { "Rial yemení", "rial yemení|riales yemenies|yer" },
            { "Fils yemení", "fils yemení|fils yemenies" },
            //Yibuti
            { "Franco yibutiano", "franco yibutiano| francos yibutianos|djf" },
            //Yugoslavia
            {"Dinar yugoslavo", "dinar yugoslavo|dinares yugoslavos|yud"},
            //Zambia
            { "Kwacha zambiano", "kwacha zambiano|kwacha zambianos|zmw" },
            { "Ngwee zambiano", "ngwee zambiano|ngwee zambianos" },
            //Zimbabue -> Dólar Estadounidense
        }.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> CurrencyPrefixList = new Dictionary<string, string>
        {
            {"Dólar", "$"},
            {"Dólar estadounidense", "us$|u$d|usd"},
            {"Dólar del Caribe Oriental", "ec$|xcd"},
            {"Dólar australiano", "a$|aud"},
            {"Dólar bahameño", "b$|bsd"},
            {"Dólar de Barbados", "bds$|bbd"},
            {"Dólar beliceño", "bz$|bzd"},
            {"Dólar bermudeño", "bd$|bmd"},
            {"Dólar de Brunéi", "brunéi $|bnd"},
            {"Dólar de Singapur", "s$|sgd"},
            {"Dólar canadienser", "c$|can$|cad"},
            {"Dólar de las Islas Caimán", "ci$|kyd"},
            {"Dólar neozelandés", "nz$|nzd"},
            {"Dólar fiyiano", "fj$|fjd"},
            {"Dólar guyanés", "gy$|gyd"},
            {"Dólar de Hong Kong", "hk$|hkd"},
            {"Dólar jamaiquino", "j$|ja$|jmd"},
            {"Dólar liberiano", "l$|lrd"},
            {"Dólar namibio", "n$|nad"},
            {"Dólar de las Islas Salomón", "si$|sbd"},
            {"Nuevo dólar taiwanés", "nt$|twd"},
            {"Real brasileño", "r$|brl" },
            {"Guaraní", "₲|gs.|pyg" },
            {"Dólar trinitense", "tt$|ttd"},
            {"Yuan chino", "￥|cny|rmb"},
            {"Yen", "¥|jpy"},
            {"Euro", "€|eur"},
            {"Florín", "ƒ"},
            {"Libra", "£|gbp"},
            {"Colón costarricense", "₡"},
            {"Lira turca", "₺"},
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
        {
            "le",
        }.ToImmutableList();
    }
}
