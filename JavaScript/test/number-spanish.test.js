var NumberRecognizer = require('../compiled/number/numberRecognizer').default;
var Culture = require('../compiled/culture').Culture;
var CultureInfo = require('../compiled/culture').CultureInfo;
var BigNumber = require('bignumber.js');
var describe = require('ava-spec').describe;

var SpanishCultureInfo = new CultureInfo(Culture.Spanish);

describe('Ordinal Model .', it => {
    let model = NumberRecognizer.instance.getOrdinalModel(Culture.Spanish, false);

    basicTest(it, model, "tresmillonesimo", "3000000");

    basicTest(it, model, "dos mil millonesimo", "2000000000");

    basicTest(it, model, "septimo", "7");

    basicTest(it, model, "cuadragesimo septimo", "47");

    basicTest(it, model, "tricentesimo cuadragesimo septimo", "347");

    basicTest(it, model, "dosmilesimo tricentesimo cuadragesimo septimo", "2347");

    basicTest(it, model, "cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "52347");

    basicTest(it, model, "cuatrocientos cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "452347");

    basicTest(it, model, "tresmillonesimo septimo", "3000007");

    basicTest(it, model, "tresmillonesimo cuadragesimo septimo", "3000047");

    basicTest(it, model, "tresmillonesimo tricentesimo cuadragesimo septimo", "3000347");

    basicTest(it, model, "tres millones dos milesimo tricentesimo cuadragesimo septimo", "3002347");

    basicTest(it, model, "tres millones cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "3052347");

    basicTest(it, model, "tres millones cuatrocientos cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "3452347");

    basicTest(it, model, "trece millones cuatrocientos cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "13452347");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos milesimo tricentesimo cuadragesimo septimo", "513452347");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos milesimo tricentesimo cuadragesimo", "513452340");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos milesimo tricentesimo", "513452300");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos milesimo", "513452000");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta milesimo", "513450000");

    basicTest(it, model, "quinientos trece millones cuatrocientos milesimo", "513400000");

    basicTest(it, model, "quinientos trece millonesimo", "513000000");

    basicTest(it, model, "quinientos diez millonesimo", "510000000");

    basicTest(it, model, "quinientosmillonesimo", "500000000");

    basicTest(it, model, "milesimo quingentesimo vigesimo tercero", "1523");

    // a little number :p
    basicTest(it, model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones quinientos cincuenta y seis milesimo octingentesimo trigesimo segundo", "3455228556832");

    basicTest(it, model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones quinientos cincuenta y seis milesimo", "3455228556000");

    basicTest(it, model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millonesimo", "3455228000000");

    basicTest(it, model, "tres billones cuatrocientos cincuenta y cinco mil millonesimo", "3455000000000");

    basicTest(it, model, "tresbillonesimo", "3000000000000");

    basicTest(it, model, "vigesimo quinto", "25");

    basicTest(it, model, "vigesimo primero", "21");

    basicTest(it, model, "centesimo vigesimo quinto", "125");

    basicTest(it, model, "ducentesimo", "200");

    basicTest(it, model, "tres mil quinientos veinticuatro millones seiscientos noventa y cuatro milesimo sexcentesimo septuagesimo tercero", "3524694673");

    basicTest(it, model, "tres mil quinientos veinticuatro millones seiscientos noventa y cuatro milesimo sexcentesimo septuagesimo", "3524694670");

    basicTest(it, model, "tres mil quinientos veinticuatro millones seiscientos noventa y cuatro milesimo sexcentesimo", "3524694600");

    basicTest(it, model, "tres mil quinientos veinticuatro millones seiscientos milesimo", "3524600000");

    basicTest(it, model, "tres mil millonesimo", "3000000000");

    basicTest(it, model, "tres mil millonesimo tercero", "3000000003");

    basicTest(it, model, "tres mil millonesimo septuagesimo tercero", "3000000073");

    basicTest(it, model, "tres mil millonesimo sexcentesimo septuagesimo tercero", "3000000673");

    basicTest(it, model, "tres mil millones cuatro milesimo sexcentesimo septuagesimo tercero", "3000004673");

    basicTest(it, model, "tres mil veinticuatro millones seiscientos noventa y cuatro milesimo sexcentesimo septuagesimo tercero", "3024694673");

    basicTest(it, model, "11mo", "11");

    basicTest(it, model, "11vo", "11");

    basicTest(it, model, "12vo", "12");

    basicTest(it, model, "111ro", "111");

    basicTest(it, model, "111ro", "111");

    basicTest(it, model, "21ro", "21");

    basicTest(it, model, "30ma", "30");

    basicTest(it, model, "2da", "2");

    basicTest(it, model, "undecimo", "11");

    basicTest(it, model, "veintidosmilesimo", "22000");

    basicTest(it, model, "cincuenta y cinco billones quinientos cincuenta y cinco mil quinientos cincuenta y cinco millones quinientos cincuenta y cinco milesimo quingentesimo quincuagesimo quinto", "55555555555555");

    basicTest(it, model, "vigesimo", "20");

    basicTest(it, model, "centesimo", "100");

    basicTest(it, model, "tres billonesimo", "3000000000000");

    basicTest(it, model, "tres billonesima", "3000000000000");

    basicTest(it, model, "cien billonesimo", "100000000000000");
});

describe('Number Model .', it => {
    let model = NumberRecognizer.instance.getNumberModel(Culture.Spanish, false);

    basicTest(it, model, "2 mil millones", "2000000000");

    basicTest(it, model, " 123456789101231", "123456789101231");

    basicTest(it, model, "-123456789101231", "-123456789101231");

    basicTest(it, model, " -123456789101231", "-123456789101231");

    basicTest(it, model, " -1", "-1");

    basicTest(it, model, "1.234.567", "1234567");

    basicTest(it, model, "3 docenas", "36");

    basicTest(it, model, "dos mil millones", "2000000000");

    basicTest(it, model, "una docena", "12");

    basicTest(it, model, "quince docenas", "180");

    basicTest(it, model, "dos mil y cuatro docenas", "2048");

    basicTest(it, model, "siete", "7");

    basicTest(it, model, "cuarenta y siete", "47");

    basicTest(it, model, "trescientos cuarenta y siete", "347");

    basicTest(it, model, "dos mil trescientos cuarenta y siete", "2347");

    basicTest(it, model, "cincuenta y dos mil trescientos cuarenta y siete", "52347");

    basicTest(it, model, "cuatrocientos cincuenta y dos mil trescientos cuarenta y siete", "452347");

    basicTest(it, model, "tres millones", "3000000");

    basicTest(it, model, "tres millones siete", "3000007");

    basicTest(it, model, "tres millones cuarenta y siete", "3000047");

    basicTest(it, model, "tres millones trescientos cuarenta y siete", "3000347");

    basicTest(it, model, "tres millones dos mil trescientos cuarenta y siete", "3002347");

    basicTest(it, model, "tres millones cincuenta y dos mil trescientos cuarenta y siete", "3052347");

    basicTest(it, model, "tres millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete", "3452347");

    basicTest(it, model, "trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete", "13452347");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete", "513452347");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta", "513452340");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos", "513452300");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos mil", "513452000");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta mil", "513450000");

    basicTest(it, model, "quinientos trece millones cuatrocientos mil", "513400000");

    basicTest(it, model, "quinientos trece millones", "513000000");

    basicTest(it, model, "quinientos diez millones", "510000000");

    basicTest(it, model, "quinientos millones", "500000000");

    basicTest(it, model, "mil quinientos veintitres", "1523");

    // a little number :p
    basicTest(it, model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones quinientos cincuenta y seis mil ochocientos treinta y dos", "3455228556832");

    basicTest(it, model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones quinientos cincuenta y seis mil", "3455228556000");

    basicTest(it, model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones", "3455228000000");

    basicTest(it, model, "tres billones cuatrocientos cincuenta y cinco mil millones", "3455000000000");

    basicTest(it, model, "tres billones", "3000000000000");

    // super number :p - supported by extractor but not by parser
    // basicTest(GetOrdinalModel(), "ciento sesenta y tres septillones quinientos ochenta y dos mil ochocientos setenta y un sextillones ciento dieciocho mil novecientos trece quintillones quinientos ochenta y cinco mil trescientos cuarenta y seis cuatrillones novecientos noventa y siete mil doscientos setenta y tres trillones cuatrocientos treinta y cuatro mil trescientos veinticinco billones quinientos cincuenta y cinco mil ochocientos veintiún millones novecientos cincuenta y tres mil seiscientos setenta y cinco", "163582871118913585346997273434325555821953675");

    // numbers within sentences
    basicTest(it, model, "dame un mil", "1000", "un mil");

    basicTest(it, model, "tirate un paso", "1", "un");

    basicTest(it, model, "voy a comprar solo una vaca", "1", "una");

    basicTest(it, model, "voy a comprar doscientas vacas", "200", "doscientas");

    basicTest(it, model, "tengo solamente mil cien pesos", "1100", "mil cien");

    basicTest(it, model, "tengo solamente siete mil doscientos treinta y cinco pesos", "7235", "siete mil doscientos treinta y cinco");

    basicTest(it, model, "no mucho, creo que voy a gastar algo asi como trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete bolivares en todo el proyecto", "13452347", "trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete");

    // Double numbers

    basicTest(it, model, " 101231,2353", "101231,2353");

    basicTest(it, model, "-101231,4323", "-101231,4323");

    basicTest(it, model, " -89101231,5127", "-89101231,5127");

    basicTest(it, model, " -1,1234567", "-1,1234567");

    basicTest(it, model, "1.234.567,51274", "1234567,51274");

    basicTest(it, model, "192,", "192", "192");

    basicTest(it, model, ",23456000", "0,23456");

    basicTest(it, model, "4,800", "4,8");

    basicTest(it, model, ",08", "0,08");

    basicTest(it, model, "9,2321312", "9,2321312");

    basicTest(it, model, " -9,2321312", "-9,2321312");

    basicTest(it, model, "1e10", "10000000000");

    basicTest(it, model, "1,1^23", "8,95430243255239");

    basicTest(it, model, "siete con cincuenta", "7,5");

    basicTest(it, model, "cuarenta y siete coma veintiocho", "47,28");

    basicTest(it, model, "trescientos cuarenta y siete con quinientos doce", "347,512");

    basicTest(it, model, "dos mil trescientos cuarenta y siete coma mil quinientos setenta y ocho", "2347,1578");

    basicTest(it, model, "cincuenta y dos mil trescientos cuarenta y siete con doscientos", "52347,2");

    basicTest(it, model, "cuatrocientos cincuenta y dos mil trescientos cuarenta y siete coma veintidos", "452347,22");

    basicTest(it, model, "1,1^+23", "8,95430243255239");

    basicTest(it, model, "2,5^-1", "0,4");

    basicTest(it, model, "-2500^-1", "-0,0004");

    basicTest(it, model, "-1,1^+23", "-8,95430243255239");

    basicTest(it, model, "-2,5^-1", "-0,4");

    basicTest(it, model, "-1,1^--23", "-8,95430243255239");

    basicTest(it, model, "-127,32e13", "-1,2732E+15");

    basicTest(it, model, "12,32e+14", "1,232E+15");

    basicTest(it, model, "-12e-1", "-1,2");

    // Translated numbers from english

    basicTest(it, model, "192.", "192", "192");

    // '.' is group separator in spanish - so not understood as IP
    multiTest(it, model, "192.168.1.2", 3);
    //this will be supported for the NumberWithUnitModel
    multiTest(it, model, "son 180,25ml liquidos", 0);

    multiTest(it, model, "son 180ml liquidos", 0);

    multiTest(it, model, " 29km caminando ", 0);

    multiTest(it, model, " subamos al 4to piso ", 0);

    multiTest(it, model, "son ,25ml liquidos", 0);

    basicTest(it, model, ",08", "0,08");

    multiTest(it, model, "uno", 1);

    multiTest(it, model, "un", 1);

    basicTest(it, model, ",23456000", "0,23456");

    basicTest(it, model, "4,800", "4,8");

    basicTest(it, model, "ciento tres con dos tercios", SpanishCultureInfo.format(103 + 2 / 3));

    basicTest(it, model, "dieciseis", "16");

    basicTest(it, model, "dos tercios", SpanishCultureInfo.format(2 / 3));

    basicTest(it, model, "ciento dieciseis", "116");

    basicTest(it, model, "ciento seis", "106");

    basicTest(it, model, "ciento sesenta y un", "161");

    basicTest(it, model, "un billonesimo", "1E-12");

    basicTest(it, model, "cien billonesimos", "1E-10");

    basicTest(it, model, " media   docena ", "6");

    basicTest(it, model, " 3 docenas", "36");

    basicTest(it, model, "una docena", "12");

    basicTest(it, model, " tres docenas ", "36");

    basicTest(it, model, "1.234.567", "1234567");

    multiTest(it, model, "1. 234. 567", 3);

    basicTest(it, model, "9,2321312", "9,2321312");

    basicTest(it, model, " -9,2321312", "-9,2321312");

    basicTest(it, model, " -1", "-1");

    basicTest(it, model, "-4/5", "-0,8");

    basicTest(it, model, "- 1 4/5", "-1,8");

    basicTest(it, model, "tres", "3");

    basicTest(it, model, " 123456789101231", "123456789101231");

    basicTest(it, model, "-123456789101231", "-123456789101231");

    basicTest(it, model, " -123456789101231", "-123456789101231");

    basicTest(it, model, "1", "1");

    basicTest(it, model, "10k", "10000");

    basicTest(it, model, "10G", "10000000000");

    basicTest(it, model, "- 10  k", "-10000");

    basicTest(it, model, "2 millones", "2000000");

    basicTest(it, model, "1 billon", "1000000000000");

    basicTest(it, model, " tres ", "3");

    basicTest(it, model, "un billon", "1000000000000");

    basicTest(it, model, "veintiun billones", "21000000000000");

    basicTest(it, model, "veintiun billones trescientos", "21000000000300");

    basicTest(it, model, "cincuenta   y   dos", "52");

    basicTest(it, model, "trescientos   treinta  y   uno", "331");

    basicTest(it, model, "doscientos dos mil", "202000");

    basicTest(it, model, "dos mil doscientos", "2200");

    basicTest(it, model, " 2,33 k", "2330");

    basicTest(it, model, " doscientos coma cero tres", "200,03");

    basicTest(it, model, " doscientos con setenta y uno", "200,71");

    basicTest(it, model, "1e10", "10000000000");

    basicTest(it, model, "1,1^23", "8,95430243255239");

    basicTest(it, model, " 322 millones ", "322000000");

    basicTest(it, model, "setenta", "70");

    basicTest(it, model, "cincuenta y dos", "52");

    basicTest(it, model, "2  1/4", "2,25");

    basicTest(it, model, "3/4", "0,75");

    basicTest(it, model, "un octavo", "0,125");

    basicTest(it, model, "cinco octavos", "0,625");

    basicTest(it, model, "un medio", "0,5");

    basicTest(it, model, "tres cuartos", "0,75");

    basicTest(it, model, "veinte con tres quintos", "20,6");

    basicTest(it, model, "veintitres quintos", "4,6");

    basicTest(it, model, "veintitres con tres quintos", "23,6");

    basicTest(it, model, "un millon dos mil doscientos tres quintos", "200440,6");

    basicTest(it, model, "uno con un medio", "1,5");

    basicTest(it, model, "uno con un cuarto", "1,25");

    basicTest(it, model, "cinco con un cuarto", "5,25");

    basicTest(it, model, "cien con tres cuartos", "100,75");

    basicTest(it, model, "un centesimo", "0,01");
});

describe('Fraction Model', it => {
    var model = NumberRecognizer.instance.getNumberModel(Culture.Spanish, false);

    basicTest(it, model, "un quinto", "0,2");

    basicTest(it, model, "un billonesimo", "1E-12");

    basicTest(it, model, "cien mil billonesima", "1E-07");

    basicTest(it, model, "tres quintos", "0,6");

    basicTest(it, model, "veinte quintos", "4");

    basicTest(it, model, "veintitres quintas", "4,6");

    basicTest(it, model, "tres con un quinto", "3,2");

    basicTest(it, model, "veintiun quintos", "4,2");

    basicTest(it, model, "un veintiunavo", SpanishCultureInfo.format(1 / 21));

    basicTest(it, model, "ciento treinta y tres veintiunavos", SpanishCultureInfo.format(133 / 21));

    basicTest(it, model, "ciento treinta con tres veintiunavos", SpanishCultureInfo.format(130 + 3 / 21));

    basicTest(it, model, "veintidos treintavos", SpanishCultureInfo.format(22 / 30));

    basicTest(it, model, "un veinticincoavo", SpanishCultureInfo.format(1 / 25));

    basicTest(it, model, "veinte veinticincoavos", "0,8");

    basicTest(it, model, "tres veintiunavos", SpanishCultureInfo.format(3 / 21));

    basicTest(it, model, "tres veinteavos", SpanishCultureInfo.format(3 / 20));

    basicTest(it, model, "tres doscientosavos", SpanishCultureInfo.format(3 / 200));

    basicTest(it, model, "tres dosmilesimos", SpanishCultureInfo.format(3 / 2000));

    basicTest(it, model, "tres veintemilesimos", SpanishCultureInfo.format(3 / 20000));

    basicTest(it, model, "tres doscientosmilesimos", SpanishCultureInfo.format(3 / 200000));

    basicTest(it, model, "tres dosmillonesimos", SpanishCultureInfo.format(3 / 2000000));

    // act like Google
    basicTest(it, model, "ciento treinta quintos", SpanishCultureInfo.format(130 / 5));

    basicTest(it, model, "cien treintaicincoavos", SpanishCultureInfo.format(100 / 35));

    // this is spanish can be interpreted as 130 + 2/5 or 132 / 5 - in this case go for 2nd option for simplicity
    basicTest(it, model, "ciento treinta y dos cincoavos", SpanishCultureInfo.format(132 / 5));

    // and we go for the first option if the user writes it using 'con'
    basicTest(it, model, "ciento treinta con dos cincoavos", SpanishCultureInfo.format(130 + 2 / 5));

    basicTest(it, model, "ciento treinta y dos quintos", SpanishCultureInfo.format(132 / 5));

    basicTest(it, model, "ciento treinta con dos quintos", SpanishCultureInfo.format(130 + 2 / 5));

    basicTest(it, model, "un cientocincoavos", SpanishCultureInfo.format(new BigNumber(1).dividedBy(105)));

    basicTest(it, model, "cien milcincoavos", SpanishCultureInfo.format(100 / 1005));

    basicTest(it, model, "uno sobre tres", SpanishCultureInfo.format(1 / 3));

    basicTest(it, model, "1 sobre 21", SpanishCultureInfo.format(1 / 21));

    basicTest(it, model, "1 sobre tres", SpanishCultureInfo.format(1 / 3));

    basicTest(it, model, "1 sobre 3", SpanishCultureInfo.format(1 / 3));

    basicTest(it, model, "uno sobre 3", SpanishCultureInfo.format(1 / 3));

    basicTest(it, model, "uno sobre 20", SpanishCultureInfo.format(1 / 20));

    basicTest(it, model, "uno sobre veinte", SpanishCultureInfo.format(1 / 20));

    basicTest(it, model, "uno sobre cien", SpanishCultureInfo.format(1 / 100));

    basicTest(it, model, "1 sobre ciento veintiuno", SpanishCultureInfo.format(1 / 121));

    basicTest(it, model, "uno sobre ciento treinta y cinco", SpanishCultureInfo.format(1 / 135));

    basicTest(it, model, "cinco medios", SpanishCultureInfo.format(5 / 2));

    basicTest(it, model, "tres cuartos", SpanishCultureInfo.format(3 / 4));

    basicTest(it, model, "dos tercios", SpanishCultureInfo.format(2 / 3));

    basicTest(it, model, "ciento treinta y cinco medios", SpanishCultureInfo.format(135 / 2));

    // not supported should be written as integer + decimal part => once con uno y medio
    // basicTest(it, model, "diez con tres medios", SpanishCultureInfo.format(10 + 3 / 2));
    basicTest(it, model, "once con uno y medio", SpanishCultureInfo.format(10 + 3 / 2));

    basicTest(it, model, "diez con un medio", SpanishCultureInfo.format(10 + 1 / 2));

    basicTest(it, model, "diez con un cuarto", SpanishCultureInfo.format(10 + 1 / 4));
});

describe('Percent Model', it => {
    var model = NumberRecognizer.instance.getPercentageModel(Culture.Spanish, false);

    basicTest(it, model, "100%", "100%");

    basicTest(it, model, " 100% ", "100%");

    basicTest(it, model, " 100 por ciento", "100%");

    basicTest(it, model, " cien por cien", "100%");

    basicTest(it, model, "cien por ciento", "100%");

    basicTest(it, model, "243 por ciento", "243%");

    basicTest(it, model, "veinte por ciento", "20%");

    basicTest(it, model, "treinta y cinco por ciento", "35%");

    basicTest(it, model, "quinientos treinta y cinco por ciento", "535%");

    basicTest(it, model, "10 por ciento", "10%");

    basicTest(it, model, "diez por ciento", "10%");

    basicTest(it, model, "tres millones cincuenta y dos mil trescientos cuarenta y siete por ciento", "3052347%");

    basicTest(it, model, "tres millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete por ciento", "3452347%");

    basicTest(it, model, "trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete por ciento", "13452347%");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta y siete por ciento", "513452347%");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos cuarenta por ciento", "513452340%");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos mil trescientos por ciento", "513452300%");

    basicTest(it, model, "quinientos trece millones cuatrocientos cincuenta y dos mil por ciento", "513452000%");

    // a little percentage :p
    basicTest(it, model, "tres billones cuatrocientos cincuenta y cinco mil doscientos veintiocho millones quinientos cincuenta y seis mil ochocientos treinta y dos por ciento", "3455228556832%");

    // percentages within sentences
    basicTest(it, model, "algo asi como un 11%", "11%", "11%");

    basicTest(it, model, "claro, solamente un 15 por ciento", "15%", "15 por ciento");

    basicTest(it, model, "si, nada mas un veinticinco por ciento", "25%", "veinticinco por ciento");

    basicTest(it, model, "todo, dejame el cien por cien del combustible", "100%", "cien por cien");

    basicTest(it, model, "un porcentaje del 25%", "25%", "25%");

    basicTest(it, model, "un porcentaje del treinta y seis por ciento del total", "36%", "treinta y seis por ciento");

    basicTest(it, model, "un porcentaje del ochenta y cuatro por cien solamente", "84%", "ochenta y cuatro por cien");

    // Double percentages

    basicTest(it, model, " 101231,2353%", "101231,2353%");

    basicTest(it, model, "-101231,4323%", "-101231,4323%");

    basicTest(it, model, " -89101231,5127 por ciento", "-89101231,5127%");

    basicTest(it, model, " -1,1234567 por cien", "-1,1234567%");

    basicTest(it, model, "1.234.567,51274 por ciento", "1234567,51274%");

    basicTest(it, model, ",23456000%", "0,23456%");

    basicTest(it, model, "4,800%", "4,8%");

    basicTest(it, model, ",08 por ciento", "0,08%");

    basicTest(it, model, "9,2321312%", "9,2321312%");

    basicTest(it, model, " -9,2321312 por cien", "-9,2321312%");

    basicTest(it, model, "1e10%", "10000000000%");

    basicTest(it, model, "1,1^23 por ciento", "8,95430243255239%");

    basicTest(it, model, "siete con cincuenta por ciento", "7,5%");

    basicTest(it, model, "cuarenta y siete coma veintiocho por ciento", "47,28%");

    basicTest(it, model, "trescientos cuarenta y siete con quinientos doce por ciento", "347,512%");

    basicTest(it, model, "dos mil trescientos cuarenta y siete coma mil quinientos setenta y ocho por ciento", "2347,1578%");

    basicTest(it, model, "cincuenta y dos mil trescientos cuarenta y siete con doscientos por ciento", "52347,2%");

    basicTest(it, model, "cuatrocientos cincuenta y dos mil trescientos cuarenta y siete coma veintidos por ciento", "452347,22%");

    // Fraction percentages

    basicTest(it, model, "tres quintos por ciento", SpanishCultureInfo.format(3 / 5) + "%");

    basicTest(it, model, "dos coma cinco por ciento", "2,5%");

    basicTest(it, model, "un quinto por ciento", "0,2%");

    basicTest(it, model, "un billonesimo por cien", "1E-12%");

    basicTest(it, model, "un veintiunavo por ciento", SpanishCultureInfo.format(1 / 21) + "%");

    basicTest(it, model, "ciento treinta y tres veintiunavos por ciento", SpanishCultureInfo.format(133 / 21) + "%");

    basicTest(it, model, "ciento treinta con tres veintiunavos por ciento", SpanishCultureInfo.format(130 + 3 / 21) + "%");

    basicTest(it, model, "veintidos treintavos por ciento", SpanishCultureInfo.format(22 / 30) + "%");

    basicTest(it, model, "tres dosmilesimos por ciento", SpanishCultureInfo.format(3 / 2000) + "%");

    basicTest(it, model, "tres veintemilesimos por ciento", SpanishCultureInfo.format(3 / 20000) + "%");

    // act like Google
    basicTest(it, model, "ciento treinta quintos por ciento", SpanishCultureInfo.format(130 / 5) + "%");

    basicTest(it, model, "cien treintaicincoavos por ciento", SpanishCultureInfo.format(100 / 35) + "%");

    // this is spanish can be interpreted as 130 + 2/5 or 132 / 5 - in this case go for 2nd option for simplicity
    basicTest(it, model, "ciento treinta y dos cincoavos por ciento", SpanishCultureInfo.format(132 / 5) + "%");

    // and we go for the first option if the user writes it using 'con'
    basicTest(it, model, "ciento treinta con dos cincoavos por ciento", SpanishCultureInfo.format(130 + 2 / 5) + "%");

    basicTest(it, model, "ciento treinta y dos quintos por ciento", SpanishCultureInfo.format(132 / 5) + "%");

    basicTest(it, model, "ciento treinta con dos quintos por ciento", SpanishCultureInfo.format(130 + 2 / 5) + "%");

    basicTest(it, model, "uno sobre tres por ciento", SpanishCultureInfo.format(1 / 3) + "%");

    basicTest(it, model, "1 sobre 3 por ciento", SpanishCultureInfo.format(1 / 3) + "%");

    basicTest(it, model, "3/4%", SpanishCultureInfo.format(3 / 4) + "%");

    basicTest(it, model, "2/3%", SpanishCultureInfo.format(2 / 3) + "%");
});

function basicTest(it, model, source, value, text) {
    it(source, t => {
        t.not(model, null);
        let result = model.parse(source);
        t.is(result.length, 1);
        t.is(result[0].text, text || source.trim());
        t.is(result[0].resolution["value"], value);
    });
}

function wrappedTest(it, model, source, extractSrc, value) {
    it(source, t => {
        t.not(model, null);
        let result = model.parse(source);
        t.is(result.length, 1);
        t.is(result[0].text, extractSrc);
        t.is(result[0].resolution["value"], value);
    });
}

function multiTest(it, model, source, count) {
    it(source, t => {
        t.not(model, null);
        let result = model.parse(source);
        t.is(result.length, count);
    });
}