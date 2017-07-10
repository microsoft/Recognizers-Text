using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class CurrencyExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public CurrencyExtractorConfiguration() : this(new CultureInfo(Culture.Portuguese)) { }

        public CurrencyExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => CurrencySuffixList;

        public override ImmutableDictionary<string, string> PrefixList => CurrencyPrefixList;

        public override ImmutableList<string> AmbiguousUnitList => AmbiguousValues;

        public override string ExtractType => Constants.SYS_UNIT_CURRENCY;

        public static readonly ImmutableDictionary<string, string> CurrencySuffixList = new Dictionary<string, string>
        {
            // Reference Sources: https://pt.wikipedia.org/wiki/Lista_de_moedas and http://repositorio.ul.pt/bitstream/10451/9948/1/ulfl139260_tm.pdf

            // General Units
            { "Dólar", "dólar|dolar|dólares|dolares" },
            { "Peso", "peso|pesos" },
            { "Coroa", "coroa|coroas" },
            { "Rublo", "rublo|rublos"},
            { "Libra", "libra|libras" },
            { "Florín", "florín|florin|floríns|florins|ƒ" },
            { "Dinar", "dinar|dinares" },
            { "Franco", "franco|francos" },
            { "Rúpia", "rúpia|rupia|rúpias|rupias" },
            { "Escudo", "escudo|escudos" },
            { "Xelim", "xelim|xelins|xelims" },
            { "Lira", "lira|liras" },
            { "Centavo", "centavo|cêntimo|centimo|centavos|cêntimos|centimo" },
            { "Centésimo", "centésimo|centésimos" },
            { "Pêni", "pêni|péni|peni|penies|pennies" },
            { "Manat", "manat|manate|mánate|man|manats|manates|mánates" },

            //Euro
            { "Euro", "euro|euros|€|eur" },
            { "Centavo de Euro", "centavo de euro|cêntimo de euro|centimo de euro|centavos de euro|cêntimos de euro|centimos de euro" },
            //Dólar do Caribe Oriental
            { "Dólar do Caribe Oriental", "dólar do Caribe Oriental|dolar do Caribe Oriental|dólares do Caribe Oriental|dolares do Caribe Oriental|dólar das Caraíbas Orientais|dolar das Caraibas Orientais|dólares das Caraíbas Orientais|dolares das Caraibas Orientais|ec$|xcd" },
            { "Centavo do Caribe Oriental", "centavo do Caribe Oriental|centavo das Caraíbas Orientais|cêntimo do Caribe Oriental|cêntimo das Caraíbas Orientais|centavos do Caribe Oriental|centavos das Caraíbas Orientais|cêntimos do Caribe Oriental|cêntimos das Caraíbas Orientais" },
            //Franco CFA da África Ocidental
            { "Franco CFA da África Ocidental", "franco CFA da África Ocidental|franco CFA da Africa Ocidental|francos CFA da África Occidental|francos CFA da Africa Occidental|franco CFA Ocidental|xof" },
            { "Centavo de CFA da África Ocidental", "centavo de CFA da Africa Occidental|centavos de CFA da África Ocidental|cêntimo de CFA da Africa Occidental|cêntimos de CFA da África Ocidental" },
            //Franco CFA da África Central
            { "Franco CFA da África Central", "franco CFA da África Central|franco CFA da Africa Central|francos CFA da África Central|francos CFA da Africa Central|franco CFA central|xaf" },
            { "Centavo de CFA da África Central", "centavo de CFA de África Central|centavos de CFA da África Central|cêntimo de CFA de África Central|cêntimos de CFA da África Central" },

            //Abcásia
            {"Apsar abcásio", "apsar abcásio|apsar abecásio|apsar abcasio|apsar|apsares"},
            //Afeganistão
            {"Afegani afegão", "afegani afegão|afegane afegão|؋|afn|afegane|afgane|afegâni|afeganis|afeganes|afganes|afegânis"},
            {"Pul", "pul|pules|puls"},
            //Albânia
            {"Lek albanês", "lek|lekë|lekes|lek albanês|leque|leques|all"},
            {"Qindarke", "qindarka|qindarkë|qindarke|qindarkas"},
            //Angola
            {"Kwanza angolano", "kwanza angolano|kwanzas angolanos|kwanza|kwanzas|aoa|kz" },
            {"Cêntimo angolano", "cêntimo angolano|cêntimo|cêntimos" },
            //Antilhas Neerlandesas or Antilhas Holandesas
            { "Florim das Antilhas Holandesas", "florim das antilhas holandesas|florim das antilhas neerlandesas|ang" },
            //Arabia Saudita
            { "Rial saudita", "rial saudita|riais sauditas|riyal saudita|riyals sauditas|riyal|riyals|sar" },
            { "Halala saudita", "halala saudita|halala|hallalah" },
            //Argélia
            { "Dinar argelino", "dinar argelino|dinares argelinos|dzd" },
            { "Cêntimo argelino", "centimo argelino|centimos argelinos|cêntimo argelino|cêntimos argelinos|centavo argelino|centavos argelinos" },
            //Argentina
            { "Peso argentino", "peso argentino|pesos argentinos|peso|pesos|ar$|ars" },
            { "Centavo argentino", "centavo argentino|centavos argentinos|centavo|ctvo.|ctvos." },
            //Armênia
            { "Dram armênio", "dram armênio|dram armênios|dram arménio|dram arménios|dram armenio|dram armenios|dram|drame|drames|դր." },
            { "Luma armênio", "luma armênio|lumas armênios|luma arménio|lumas arménios|luma armenio|lumas armenios|luma|lumas" },
            //Aruba
            { "Florim arubano", "florín arubeño|florines arubeños|ƒ arubeños|aƒ|awg" },
            //Austrália
            { "Dólar australiano", "dólar australiano|dólares australianos|dolar australiano|dolares australianos|a$|aud" },
            { "Centavo australiano", "centavo australiano|centavos australianos" },
            //Azerbaijão
            {"Manat azeri", "manat azeri|manats azeris|azn|manat azerbaijanês|manat azerbaijano|manats azerbaijaneses|manats azerbaijanos" },
            {"Qəpik azeri", "qəpik azeri|qəpik|qəpiks" },
            //Bahamas
            { "Dólar bahamense", "dólar bahamense|dólares bahamense|dolar bahamense|dolares bahamense|dólar baamiano|dólares baamiano|dolar baamiano|dolares baamiano|b$|bsd" },
            { "Centavo bahamense", "centavo bahamense|centavos bahamense" },
            //Barein
            { "Dinar bareinita", "dinar bareinita|dinar baremita|dinares bareinitas|dinares baremitas|bhd" },
            { "Fil bareinita", "fil bareinita|fil baremita|fils bareinitas|fils baremitas" },
            //Bangladesh
            { "Taka bengali", "taka bengali|takas bengalis|taca|tacas|taka|takas|bdt" },
            { "Poisha bengali", "poisha bengali|poishas bengalis" },
            //Barbados
            { "Dólar de Barbados", "dólar de barbados|dólares de barbados|dolar de barbados|dolares de barbados|dólar dos barbados|dólares dos barbados|bbd" },
            { "Centavo de Barbados", "centavo de barbados|centavos de barbados|centavo dos barbados|centavos dos barbados" },
            //Belize
            { "Dólar de Belize", "dólar de belize|dólares de belize|dolar de belize|dolares de belize|dólar do belize|dólares do belize|dolar do belize|dolares do belize|bz$|bzd" },
            { "Centavo de Belize", "centavo de belize|centavos de belize|cêntimo do belize|cêntimos do belize" },
            //Bermudas
            { "Dólar bermudense", "dólar bermudense|dólares bermudenses|bd$|bmd" },
            { "Centavo bermudense", "centavo bermudense|centavos bermudenses|cêntimo bermudense| cêntimos bermudenses" },
            //Bielorrúsia / Belarus
            { "Rublo bielorruso", "rublo bielorruso|rublos bielorrusos|br|byr" },
            { "Copeque ruso", "copeque bielorruso|copeques bielorrusos|kopek bielorruso|kopeks bielorrusos|kap" },
            //Mianmar/Birmânia
            { "Quiate mianmarense", "quiate mianmarense|quiates mianmarenses|kyat mianmarense|kyates mianmarenses|quiate myanmarense|quiates myanmarenses|kyat myanmarense|kyates myanmarenses|quiate birmanês|quite birmanes|quiates birmaneses|kyat birmanês|kyat birmanes|kyates birmaneses|mmk" },
            { "Pya mianmarense", "pya mianmarense|pyas mianmarenses|pya myanmarense|pyas myanmarenses|pya birmanês|pya birmanes|pyas birmaneses" },
            //Bolívia
            { "Boliviano", "boliviano|bolivianos|bob|bs" },
            { "Centavo Boliviano", "centavo boliviano|centavos bolivianos" },
            //Bósnia e Herzegovina
            { "Marco da Bósnia e Herzegovina", "marco conversível|marco conversivel|marco convertível|marco convertivel|marcos conversíveis|marcos conversiveis|marcos convertíveis|marcos convertivies|bam" },
            { "Fening da Bósnia e Herzegovina", "fening conversível|fening conversivel|fening convertível|fening convertivel|fenings conversíveis|fenings conversiveis|fenings convertíveis|fenings convertiveis" },
            //Botsuana
            { "Pula", "pula|pulas|bwp" },
            { "Thebe", "thebe|thebes" },
            //Brasil
            { "Real brasileiro", "real brasileiro|real do brasil|real|reais brasileiros|reais do brasil|reais|r$|brl" },
            { "Centavo brasileiro", "centavo de real|centavo brasileiro|centavos de real|centavos brasileiros" },
            //Brunei
            { "Dólar de Brunéi", "dólar de brunei|dolar de brunei|dólar do brunei|dolar do brunei|dólares de brunéi|dolares de brunei|dólares do brunei|dolares do brunei|bnd" },
            { "Sen de Brunéi", "sen de brunei|sen do brunei|sens de brunei|sens do brunei" },
            //Bulgária
            { "Lev búlgaro", "lev búlgaro|leve búlgaro|leves búlgaros|lev bulgaro|leve bulgaro|leves bulgaros|lv|bgn" },
            { "Stotinka búlgaro", "stotinka búlgaro|stotinki búlgaros|stotinka bulgaro|stotinki bulgaros" },
            //Burundi
            { "Franco do Burundi", "franco do burundi|francos do burundi|fbu|fib" },
            { "Centavo Burundi", "centavo burundi|cêntimo burundi|centimo burundi|centavos burundi|cêntimo burundi|centimo burundi" },
            //Butão
            { "Ngultrum butanês", "ngultrum butanês|ngultrum butanes|ngúltrume butanês|ngultrume butanes|ngultrum butaneses|ngúltrumes butaneses|ngultrumes butaneses|btn" },
            { "Chetrum  butanês", "chetrum butanês|chetrum butanes|chetrum butaneses" },
            //Cabo Verde
            { "Escudo cabo-verdiano", "escudo cabo-verdiano|escudos cabo-verdianos|cve" },
            //Camboja
            { "Riel cambojano", "riel cambojano|riéis cambojanos|rieis cambojanos|khr" },
            //Canadá
            { "Dólar canadense", "dólar canadense|dolar canadense|dólares canadenses|dolares canadenses|c$|cad" },
            { "Centavo canadense", "centavo canadense|centavos canadenses" },
            //Chile
            { "Peso chileno", "peso chileno|pesos chilenos|cpl" },
            //China
            { "Yuan chinês", "yuan chinês|yuan chines|yuans chineses|yuan|yuans|renminbi|rmb|cny|¥" },
            //Colômbia
            { "Peso colombiano", "peso colombiano|pesos colombianos|cop|col$" },
            { "Centavo colombiano", "centavo colombiano|centavos colombianos" },
            //Comores
            { "Franco comorense", "franco comorense|francos comorenses|kmf|₣" },
            //Congo, República Democrática do
            { "Franco congolês", "franco congolês|franco congoles|francos congoleses|cdf" },
            { "Centavo congolês", "centavo congolês|centavo congoles|centavos congoleses|cêntimo congolês|centimo congoles|cêntimos congoleses|cêntimos congoleses" },
            //Coréia do Norte
            { "Won norte-coreano", "won norte-coreano|wŏn norte-coreano|won norte-coreanos|wŏn norte-coreanos|kpw" },
            { "Chon norte-coreano", "chon norte-coreano|chŏn norte-coreano|chŏn norte-coreanos|chon norte-coreanos" },
            //Coréia do Sur
            { "Won sul-coreano", "wŏn sul-coreano|won sul-coreano|wŏnes sul-coreanos|wones sul-coreanos|krw" },
            { "Jeon sul-coreano", "jeons sul-coreano|jeons sul-coreanos" },
            //Costa Rica
            { "Colón costarriquenho", "colón costarriquenho|colon costarriquenho|colons costarriquenho|colones costarriquenhos|crc" },
            //Croácia
            { "Kuna croata", "kuna croata|kunas croatas|hrk" },
            { "Lipa croata", "lipa croata|lipas croatas" },
            //Cuba
            { "Peso cubano", "peso cubano|pesos cubanos|cup" },
            { "Peso cubano convertível", "peso cubano conversível|pesos cubanos conversíveis|peso cubano conversivel|pesos cubanos conversiveis|peso cubano convertível|pesos cubanos convertíveis|peso cubano convertivel|pesos cubanos convertiveis|cuc" },
            //Dinamarca
            { "Coroa dinamarquesa", "coroa dinamarquesa|coroas dinamarquesas|dkk" },
            //Egito
            { "Libra egípcia", "libra egípcia|libra egipcia|libras egípcias|libras egipcias|egp|le" },
            { "Piastra egípcia", "piastra egípcia|piastra egipcia|pisastras egípcias|piastras egipcias" },
            //Emirados Árabes Unidos
            { "Dirham dos Emirados Árabes Unidos", "dirham|dirhams|dirham dos emirados arabes unidos|aed|dhs" },
            //Eritréia
            { "Nakfa", "nakfa|nfk|ern" },
            { "Centavo de Nakfa", "cêntimo de nakfa|cêntimos de nakfa|centavo de nafka|centavos de nafka" },
            //Espanha -> Euro + old peseta
            { "Peseta", "peseta|pesetas|pts.|ptas.|esp" },
            //Estados Unidos
            { "Dólar estadunidense", "dólar dos estados unidos|dolar dos estados unidos|dólar estadunidense|dólar americano|dólares dos estados unidos|dolares dos estados unidos|dólares estadunidenses|dólares americanos|dolar estadunidense|dolar americano|dolares estadunidenses|dolares americanos|usd|u$d|us$" },
            //Estônia -> Euro + old crown
            { "Coroa estoniana", "coroa estoniana|coroas estonianas|eek" },
            { "Senti estoniano", "senti estoniano|senti estonianos" },
            //Etiópia
            { "Birr etíope", "birr etíope|birr etiope|birr etíopes|birr etiopes|br|etb" },
            { "Santim etíope", "santim etíope|santim etiope|santim etíopes|santim etiopes" },
            //Filipinas
            { "Peso filipino", "peso filipino|pesos filipinos|php" },
            //Finlandia -> Euro + old mark
            { "Marco finlandês", "marco finlandês|marco finlandes|marcos finlandeses" },
            //Fiji
            { "Dólar fijiano", "dólar fijiano|dolar fijiano|dólares fijianos|dolares fijianos|fj$|fjd" },
            { "Centavo fijiano", "centavo fijiano|centavos fijianos" },
            //Gâmbia
            { "Dalasi gambiano", "dalasi|gmd" },
            { "Bututs", "butut|bututs" },
            //Geórgia
            { "Lari georgiano", "lari georgiano|lari georgianos|gel" },
            { "Tetri georgiano", "tetri georgiano|tetri georgianos" },
            //Gana
            { "Cedi", "cedi|ghs|gh₵" },
            { "Pesewa", "pesewa" },
            //Gibraltar
            { "Libra de Gibraltar", "libra de gibraltar|libras de gibraltar|gip" },
            { "Peni de Gibraltar", "peni de gibraltar|penies de gibraltar" },
            //Guatemala
            { "Quetzal guatemalteco", "quetzal guatemalteco|quetzales guatemaltecos|quetzal|quetzales|gtq" },
            { "Centavo guatemalteco", "centavo guatemalteco|centavos guatemaltecos" },
            //Guernsey
            { "Libra de Guernsey", "libra de Guernsey|libras de Guernsey|ggp" },
            { "Peni de Guernsey", "peni de Guernsey|penies de Guernsey" },
            //Guiné // @HERE
            { "Franco da Guiné", "franco da guiné|franco da guine| franco guineense|francos da guiné|francos da guine|francos guineense|gnf|fg" },
            { "Centavo da Guiné", "cêntimo guineense|centimo guineense|centavo guineense|cêntimos guineenses|centimos guineenses|centavos guineenses" },
            //Guiana
            { "Dólar guianense", "dólar guianense|dólares guianense|dolar guianense|dolares guianense|gyd|gy" },
            //Haiti
            { "Gurde haitiano", "gurde haitiano|gourde|gurdes haitianos|htg" },
            { "Centavo haitiano", "cêntimo haitiano|cêntimos haitianos|centavo haitiano|centavos haitianos" },
            //Honduras
            { "Lempira hondurenha", "lempira hondurenha|lempiras hondurenhas|lempira|lempiras|hnl" },
            { "Centavo hondurenho", "centavo hondurenho|centavos hondurehos|cêntimo hondurenho|cêntimos hondurenhos" },
            //Hong Kong
            { "Dólar de Hong Kong", "dólar de hong kong|dolar de hong kong|dólares de hong kong|dolares de hong kong|hk$|hkd" },
            //Hungría
            { "Florim húngaro", "florim húngaro|florim hungaro|florins húngaros|florins hungaros|forinte|forintes|huf" },
            { "Filér húngaro", "fillér|filér|filler|filer" },
            //Índia
            { "Rupia indiana", "rúpia indiana|rupia indiana|rupias indianas|inr" },
            { "Paisa indiana", "paisa indiana|paisas indianas" },
            //Indonésia
            { "Rupia indonésia", "rupia indonesia|rupia indonésia|rupias indonesias|rupias indonésias|idr" },
            { "Sen indonésio", "send indonésio|sen indonesio|sen indonésios|sen indonesios" },
            //Irã
            { "Rial iraniano", "rial iraniano|riais iranianos|irr" },
            //Iraque
            { "Dinar iraquiano", "dinar iraquiano|dinares iraquianos|iqd" },
            { "Fil iraquiano", "fil iraquiano|fils iraquianos|files iraquianos" },
            //Ilha de Man
            { "Libra manesa", "libra manesa|libras manesas|imp" },
            { "Peni manês", "peni manes|peni manês|penies maneses" },
            //Islândia
            { "Coroa islandesa", "coroa islandesa|coroas islandesas|isk|íkr" },
            { "Aurar islandês", "aurar islandês|aurar islandes|aurar islandeses|eyrir" },
            //Ilhas Cayman
            { "Dólar das Ilhas Cayman", "dólar das ilhas cayman|dolar das ilhas cayman|dólar das ilhas caimão|dólares das ilhas cayman|dolares das ilhas cayman|dólares das ilhas caimão|ci$|kyd" },
            //Ilhas Cook
            { "Dólar das Ilhas Cook", "dólar das ilhas cook|dolar das ilhas cook|dólares das ilhas cook|dolares das ilhas cook" },
            //Ilhas Féroe/Faroé/Faroés
            { "Coroa feroesa", "coroa feroesa|coroas feroesas|fkr" },
            //Ilhas Malvinas
            { "Libra das Malvinas", "libra das malvinas|libras das malvinas|fk£|fkp" },
            //Ilhas Salomão
            { "Dólar das Ilhas Salomão", "dólar das ilhas salomão|dolar das ilhas salomao|dólares das ilhas salomão|dolares das ilhas salomao|sbd" },
            //Israel
            { "Novo shekel israelense", "novo shekel|novos shekeles|novo shequel|novo siclo|novo xéquel|shekeles novos|novos sheqalim|sheqalim novos|ils" },
            { "Agora", "agora|agorot" },
            //Jamaica
            { "Dólar jamaicano", "dólar jamaicano|dolar jamaicano|dólares jamaicanos|dolares jamaicanos|j$|ja$|jmd" },
            //Japão
            { "Yen", "yen|iene|yenes|ienes|jpy" },
            //Jersey
            { "Libra de Jersey", "libra de Jersey|libras de Jersey|jep" },
            //Jordânia
            { "Dinar jordaniano", "dinar jordaniano|dinar jordano|dinares jordanianos|dinares jordanos|jd|jod" },
            { "Piastra jordaniana", "piastra jordaniana|piastra jordano|piastras jordanianas|piastra jordaniano|piastras jordanianos|piastras jordanos" },
            //Cazaquistão
            { "Tengue cazaque", "tenge|tengue|tengué|tengue cazaque|kzt" },
            { "Tiyin", "tiyin|tiyins"},
            //Quênia
            { "Xelim queniano", "xelim queniano|xelins quenianos|ksh|kes" },
            //Quirguistão
            { "Som quirguiz", "som quirguiz|som quirguizes|soms quirguizes|kgs" },
            { "Tyiyn", "tyiyn|tyiyns" },
            //Kiribati
            { "Dólar de Kiribati", "dólar de kiribati|dolar de kiribati|dólares de kiribati|dolares de kiribati" },
            //Kuwait/Cuaite
            { "Dinar kuwaitiano", "dinar kuwaitiano|dinar cuaitiano|dinares kuwaitiano|dinares cuaitianos|kwd" },
            //Laos/Laus
            { "Quipe laosiano", "quipe|quipes|kipe|kipes|kip|kip laosiano|kip laociano|kips laosianos|kips laocianos|lak" },
            { "Att laosiano", "at|att|att laosiano|att laosianos" },
            //Lesoto
            { "Loti do Lesoto", "loti|lóti|maloti|lotis|lótis|lsl" },
            { "Sente", "sente|lisente" },
            //Líbano
            { "Libra libanesa", "libra libanesa|libras libanesas|lbp" },
            //Libéria
            { "Dólar liberiano", "dólar liberiano|dolar liberiano|dólares liberianos|dolares liberianos|l$|lrd" },
            //Líbia
            { "Dinar libio", "dinar libio|dinar líbio|dinares libios|dinares líbios|ld|lyd" },
            { "Dirham libio", "dirham libio|dirhams libios|dirham líbio|dirhams líbios" },
            //Lituania -> Euro + old lita
            {"Litas lituana", "litas lituana|litai lituanas|ltl" },
            //Macao
            { "Pataca macaense", "pataca macaense|patacas macaenses|mop$|mop" },
            { "Avo macaense", "avo macaense|avos macaenses" },
            { "Ho macaense", "ho macaense|ho macaenses" },
            //Macedônia, República de
            { "Dinar macedônio", "denar macedonio|denare macedonios|denar macedônio|denar macedónio|denare macedônio|denare macedónio|dinar macedonio|dinar macedônio|dinar macedónio|dinares macedonios|dinares macedônios|dinares macedónios|den|mkd" },
            { "Deni macedônio", "deni macedonio|deni macedônio|deni macedónio|denis macedonios|denis macedônios|denis macedónios" },
            //Madagáscar
            { "Ariary malgaxe", "ariai malgaxe|ariary malgaxe|ariary malgaxes|ariaris|mga" },
            { "Iraimbilanja", "iraimbilanja|iraimbilanjas" },
            //Malásia
            { "Ringuite malaio", "ringgit malaio|ringgit malaios|ringgits malaios|ringuite malaio|ringuites malaios|rm|myr" },
            { "Sen malaio", "sen malaio|sen malaios|centavo malaio|centavos malaios|cêntimo malaio|cêntimos malaios" },
            //Malawi/Maláui
            { "Kwacha do Malawi", "kwacha|cuacha|quacha|mk|mwk" },
            { "Tambala", "tambala|tambalas|tambala malawi" },
            //Maldivas
            { "Rupia maldiva", "rupia maldiva|rupias maldivas|rupia das maldivas| rupias das maldivas|mvr" },
            //Marrocos
            { "Dirame marroquino", "dirame marroquino|dirham marroquinho|dirhams marroquinos|dirames marroquinos|mad" },
            //Maurício/Maurícia
            { "Rupia maurícia", "rupia maurícia|rupia de Maurício|rupia mauricia|rupia de mauricio|rupias de mauricio|rupias de maurício|rupias mauricias|rupias maurícias|mur" },
            //Mauritânia
            { "Uguia", "uguia|uguias|oguia|ouguiya|oguias|mro" },
            { "Kume", "kumes|kume|khoums" },
            //México
            { "Peso mexicano", "peso mexicano|pesos mexicanos|mxn" },
            { "Centavo mexicano", "centavo mexicano|centavos mexicanos" },
            //Moldávia
            { "Leu moldávio", "leu moldavo|lei moldavos|leu moldávio|leu moldavio|lei moldávios|lei moldavios|leus moldavos|leus moldavios|leus moldávios|mdl" },
            { "Ban moldávio", "ban moldavo|bani moldavos" },
            //Mongólia
            { "Tugrik mongol", "tugrik mongol|tugrik|tugriks mongóis|tugriks mongois|tug|mnt" },
            //Moçambique
            { "Metical moçambicao", "metical|metical moçambicano|metical mocambicano|meticais|meticais moçambicanos|meticais mocambicanos|mtn|mzn" },
            //Namíbia
            { "Dólar namibiano", "dólar namibiano|dólares namibianos|dolar namibio|dolares namibios|n$|nad" },
            { "Centavo namibiano", "centavo namibiano|centavos namibianos|centavo namibio|centavos namibianos" },
            //Nepal
            { "Rupia nepalesa", "rupia nepalesa|rupias nepalesas|npr" },
            { "Paisa nepalesa", "paisa nepalesa|paisas nepalesas" },
            //Nicarágua //@HERE
            { "Córdoba nicaragüense", "córdoba nicaragüense|córdobas nicaragüenses|c$|nio" },
            { "Centavo nicaragüense", "centavo nicaragüense|centavos nicaragüenses" },
            //Nigeria
            { "Naira", "naira|ngn" },
            { "Kobo", "kobo" },
            //Noruega
            { "Corona noruega", "corona noruega|coronas noruegas|nok" },
            //Nueva Caledonia
            { "Franco CFP", "franco cfp|francos cfp|xpf" },
            //Nueva Zelanda
            { "Dólar neozelandés", "dólar neozelandés|dólares neozelandeses|dólar de Nueva Zelanda|dólares de Nueva Zelanda|nz$|nzd" },
            { "Centavo neozelandés", "centavo neozelandés|centavo de Nueva Zelanda|centavos de Nueva Zelanda|centavos neozelandeses" },
            //Oman
            { "Rial omaní", "rial omaní|riales omanies|omr" },
            { "Baisa omaní", "baisa omaní|baisa omanies" },
            //Países Baixos/Holanda -> Euro + old florin
            { "Florim holandês", "florim holandês|florim holandes|florins holandeses|nlg" },
            //Paquistão
            { "Rupia pakistaní", "rupia pakistaní|rupias pakistanies|pkr" },
            { "Paisa pakistaní", "paisa pakistaní|paisas pakistanies" },
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
            { "Rublo russo", "rublo russo|rublos russos|rub|р." },
            { "Copeque ruso", "copeque russo|copeques russos|kopek ruso|kopeks rusos|copeque|copeques|kopek|kopeks" },
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
            //África do Sul
            { "Rand sulafricano", "rand|rand sulafricano|zar" },
            //Sudão
            { "Libra sudanesa", "libra sudanesa|libras sudanesas|sdg" },
            { "Piastra sudanesa", "piastra sudanesa|piastras sudanesas" },
            //Sudão do Sur
            { "Libra sursudanesa", "libra sursudanesa|libras sursudanesa|ssp" },
            { "Piastra sursudanesa", "piastra sursudanesa|piastras sursudanesas" },
            //Suéciama
            { "Coroa sueca", "coroa sueca|coroas suecas|sek" },
            //Suíça
            { "Franco suíço", "franco suíço|franco suico|francos suíços|francos suicos|sfr|chf" },
            { "Rappen suíço", "rappen suíço|rappen suico|rappens suíços|rappens suicos" },
            //Suriname
            { "Dólar surinamês", "dólar surinamês|dolar surinames|dólar do Suriname|dolar do Suriname|dólares surinameses|dolares surinameses|dólares do Suriname|dolares do Suriname|srd" },
            { "Centavo surinamês", "centavo surinamês|centavo surinames|centavos surinameses" },
            //Tailândia
            { "Baht tailandês", "baht tailandês|bath tailandes|baht tailandeses|thb" },
            { "Satang tailandês", "satang tailandês|satang tailandes|satang tailandeses" },
            //Taiwan
            { "Novo dólar taiwanês", "novo dólar taiwanês|novo dolar taiwanes|dólar taiwanês|dolar taiwanes|dólares taiwaneses|dolares taiwaneses|twd" },
            { "Centavo taiwanês", "centavo taiwanês|centavo taiwanes|centavos taiwaneses" },
            //Tanzânia
            { "Xelim tanzaniano", "xelim tanzano|xelins tanzanos|tzs" },
            { "Centavo tanzaniano", "centavo tanzano|centavos tanzanos" },
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
            {"Dólar estadunidense", "us$|u$d|usd"},
            {"Dólar del Caribe Oriental", "ec$|xcd"},
            {"Dólar australiano", "a$|aud"},
            {"Dólar bahameño", "b$|bsd"},
            {"Dólar de Barbados", "bds$|bbd"},
            {"Dólar beliceño", "bz$|bzd"},
            {"Dólar bermudeño", "bd$|bmd"},
            {"Dólar de Brunéi", "brunéi $|bnd"},
            {"Dólar de Singapur", "s$|sgd"},
            {"Dólar canadense", "c$|can$|cad"},
            {"Dólar de las Islas Caimán", "ci$|kyd"},
            {"Dólar neozelandés", "nz$|nzd"},
            {"Dólar fiyiano", "fj$|fjd"},
            {"Dólar guyanés", "gy$|gyd"},
            {"Dólar de Hong Kong", "hk$|hkd"},
            {"Dólar jamaiquino", "j$|ja$|jmd"},
            {"Dólar liberiano", "l$|lrd"},
            {"Dólar namibio", "n$|nad"},
            {"Dólar das Ilhas Salomão", "si$|sbd"},
            {"Novo dólar de Taiwan", "nt$|twd"},
            {"Real brasileiro", "r$|brl" },
            {"Guaraní", "₲|gs.|pyg" },
            {"Dólar trinitense", "tt$|ttd"},
            {"Yuan chinês", "￥|cny|rmb"},
            {"Yen", "¥|jpy"},
            {"Euro", "€|eur"},
            {"Florín", "ƒ"},
            {"Libra", "£|gbp"},
            {"Colón costarricense", "₡"},
            {"Lira turca", "₺"},
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> AmbiguousValues = new List<string>
        {
            "le",
        }.ToImmutableList();
    }
}
