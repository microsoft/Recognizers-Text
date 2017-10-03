var NumberWithUnitRecognizer = require('../compiled/numberWithUnit/numberWithUnitRecognizer').default;
var Culture = require('../compiled/culture').Culture;
var CultureInfo = require('../compiled/culture').CultureInfo;
var BigNumber = require('bignumber.js');
var describe = require('ava-spec').describe;

var SpanishCultureInfo = new CultureInfo(Culture.Spanish);

describe('Currency .', it => {
    let model = NumberWithUnitRecognizer.instance.getCurrencyModel(Culture.Spanish, false);

    basicTest(it, model,
        "Condado de Montgomery, md. - - $ 75 millones de obligaciones generales, Serie b , bonos consolidados de mejoramiento público de 1989 , A través de un Manufacturers Hanover Trust co. group.",
        "75000000 Dólar");

    basicTest(it, model,
        "Conglomerado finlandés nokia ( oy ab ) dijo que llegó a un acuerdo para comprar la compañía de cable holandés NKF kabel b.v. por 420 millones de marcos finlandeses",
        "420000000 Marco finlandés");

    basicTest(it, model,
        "Nacional pagó a Siegel y Shuster $ 94.000 para cancelar todas las reclamaciones.",
        "94000 Dólar");

    basicTest(it, model,
        "Servicios de dinámica general co., una unidad de Dinámica General corp., ganó un contrato del ejército de $ 48,2 millones para establecer facilidades del mantenimiento para los vehículos con seguimiento en Paquistán.",
        "48200000 Dólar");

    basicTest(it, model,
        "El precio del segundo simulador oscila entre C$ 16,4 millones",
        "16400000 Dólar canadiense");

    basicTest(it, model,
        "Golar Gas Holding co., una subsidiaria de Gotaas-Larsen Shipping corp., ofreciendo $ 280 millones por las notas preferidas de la hipoteca de buques, vía los mercados de capitales de Merrill Linch.",
        "280000000 Dólar");

    basicTest(it, model,
        "Bard/Ems tenía 1988 ventas de cerca de $ 14 millones, según Birtcher.",
        "14000000 Dólar");

    basicTest(it, model,
        "Los precios del acuerdo comienzan en $ 12.345.",
        "12345 Dólar");

    basicTest(it, model,
        "solamente Batman ha acumulado mas de $247 millones en taquilla hasta la fecha, convirtiendola en la película con mejor recaudación de Warner Bros.",
        "247000000 Dólar");

    basicTest(it, model,
        "El patrimonio neto de Coyle fue estimado en £ 8,10 millones en Octuble del 2014.",
        "8100000 Libra");

    basicTest(it, model,
        "Los ingresos netos por intereses cayeron un 27% en el trimestre a $ 254 millones",
        "254000000 Dólar");

    basicTest(it, model,
        "Un tribunal de apelaciones federal anuló una regulación de gas natural que había impedido que las compañías de gasoductos pasaran a los clientes un gasto de $ un mil millones en costos de contratos controversiales",
        "1000000000 Dólar");

    basicTest(it, model,
        "El trimestre de 1988 también incluyó ganancias únicas por un total de aproximadamente $ 35 millones.",
        "35000000 Dólar");

    basicTest(it, model,
        "Y.J.Park y su familia subsistió durante cuatro años para comprar un pequeño apartamento aquí, pero se encontró que cuanto más cerca estaban de llegar a ahorrar los $ 40.000 que originalmente necesitaban, más subia el precio.",
        "40000 Dólar");

    basicTest(it, model,
        "E. Robert Wallach fue sentenciado por un juez en Nueva York a seis años de prisión y una multa de $250.000 por su extorsión en el escándalo de Wedtech.",
        "250000 Dólar");

    basicTest(it, model,
        "Un artículo publicado el miércoles en la encuesta económica de Oriente Medio revela que Irak pidió a sus clientes que paguen 50 centavos más por barril de petróleo sobre el precio oficial del petróleo al 1 de diciembre en una cuenta que no está bajo la supervisión de las naciones unidas.",
        "50 Centavo");

    basicTest(it, model,
        "La división Chevrolet de General Motors Corp., reaccionando a las ventas lentas, dijo que ofrecerá rebajas de $ 800 en su Beretta 1990, la versión de dos puertas de su línea base de autos compactos.",
        "800 Dólar");

    basicTest(it, model,
        "(El almacenista también tomó $ 125 millones de bonos Junior SCI TV como pago parcial para los activos de TV).",
        "125000000 Dólar");

    basicTest(it, model,
        "En el mercado nacional de venta libre, las acciones de Scimed cayeron 2,75 dólares.",
        "2,75 Dólar");

    basicTest(it, model,
        "Al mismo tiempo, los inversionistas estiman que la reestructuración reduciría la factura anual de intereses en efectivo de la compañía en aproximadamente U$D 90 millones.",
        "90000000 Dólar estadounidense");

    basicTest(it, model,
        "Los gastos de capital en 1990 aumentarán ligeramente, dijo Mr.Marous, de un estimado de $ 470 millones este año",
        "470000000 Dólar");

    basicTest(it, model,
        "Shearson \"realmente solo tiene $ 300 millones de capital\", dice el sr. Bowman de S&P.",
        "300000000 Dólar");

    basicTest(it, model,
        "Puede ser directo (él quiere el dinero para alimento) o increíblemente enrevesado; su hermana está en este momento cerca de la muerte en Hoboken, ha perdido su cartera y tiene sólo $ 1,22 en cambio para poner un billete de autobús, y no le das la diferencia?",
        "1,22 Dólar");

    basicTest(it, model,
        "El contrato de diciembre subió 1,20 centavos",
        "1,2 Centavo");

    basicTest(it, model,
        "Walter Kirchberger, un analista de Painewebber inc., dijo que ofrecer a los interesados un precio más alto de $ 70 por acción es \"un método bastante efectivo de bloquear\" la oferta.",
        "70 Dólar");

    basicTest(it, model,
        "Las ventas netas para el tercer trimestre de este año fueron de $ 14 millones más que el año pasado.",
        "14000000 Dólar");

    basicTest(it, model,
        "La compañía matriz del primer banco nacional de Chicago, con 48.000 millones de dólares en activos, dijo que se reservó de absorber pérdidas en préstamos e inversiones en países con dificultades financieras.",
        "48000000000 Dólar");

    basicTest(it, model,
        "Fluor Corp. dijo que se le adjudicó un contrato de $ 300 millones para prestar servicios de ingeniería y gestión de la construcción en una mina de cobre en Irian Jaya, Indonesia, para una unidad de Freeport-McMoran Copper co.",
        "300000000 Dólar");

    basicTest(it, model,
        "La bolsa americana dijo que un asiento fue vendido por $ 5.000 desde la venta anterior el viernes pasado.",
        "5000 Dólar");

    basicTest(it, model,
        "Warner Communications Inc., que está siendo adquirida por Time Warner, ha presentado una demanda por violación de contrato de un mil millones de dólares contra Sony y dos productores.",
        "1000000000 Dólar");

    basicTest(it, model,
        "En agosto, Asarco, a través de su subsidiaria Lac d'amiante Du Québec, vendió el interés restante de un tercio en una sociedad limitada minera de amianto en Canadá por $ 11,7 millones.",
        "11700000 Dólar");

    basicTest(it, model,
        "En 1988, las exportaciones de juguetes y juegos de producción nacional cayeron un 19% desde 1987 hasta alcanzar los 10050 millones de dólares de Hong Kong.",
        "10050000000 Dólar de Hong Kong");

    basicTest(it, model,
        "Las ventas del cuarto trimestre fiscal crecieron cerca de 18% a $ 1,17 mil millones en comparacion al año anterior.",
        "1170000000 Dólar");

    basicTest(it, model,
        "Durante la primera hora de ayer, los precios cayeron hasta 1/4 de punto, o bajaron alrededor de $ 2,50 por cada monto nominal.",
        "2,5 Dólar");

    basicTest(it, model,
        "New Jersey, por ejemplo, se le pidió que aceptara $ 300.000, pero se negó",
        "300000 Dólar");

    basicTest(it, model,
        "Las ventas subieron 6,2% a $ 1,45 mil millones",
        "1450000000 Dólar");

    basicTest(it, model,
        "A partir de ayer por la tarde, los reembolsos representaron menos del 15% de la posición total de efectivo de alrededor de $ 2 mil millones de los fondos de acciones de fidelidad.",
        "2000000000 Dólar");

    basicTest(it, model,
        "Onvia. Com Inc., bajó 34 centavos",
        "34 Centavo");

    basicTest(it, model,
        "El nuevo folleto dice que si la adquisición hubiera sido completada antes, las ganancias antes de impuestos \"habrían sido insuficientes para cubrir sus cargos fijos, incluyendo intereses sobre títulos de deuda\", por aproximadamente $ 62,7 millones en el primer semestre de 1989.",
        "62700000 Dólar");

    basicTest(it, model,
        "Filenet señaló que tenía efectivo y valores negociables por un total de $ 22,5 millones en septiembre.",
        "22500000 Dólar");

    basicTest(it, model,
        "Para los 20 restaurantes más caros de la ciudad, el precio de una cena aumentó a $ 63,45, también hubo un aumento del 8 por ciento.",
        "63,45 Dólar");

    basicTest(it, model,
        "Trans Mundo Airlines Inc., ofreciendo billetes senior de 150 millones de dólares, a través de Drexel Burnham.",
        "150000000 Dólar");

    basicTest(it, model,
        "El fettuccine con champiñones portobello cuesta $ 8,50.",
        "8,5 Dólar");

    basicTest(it, model,
        "El delivery de marzo terminó con un anticipo de 14,27 centavos",
        "14,27 Centavo");

    basicTest(it, model,
        "En el tercer trimestre de 1988 fue de $ 75,3 millones",
        "75300000 Dólar");

    basicTest(it, model,
        "La confianza de los demandantes del protector del dalkon $ 2,38 mil millones fue establecida",
        "2380000000 Dólar");

    basicTest(it, model,
        "Los términos de la oferta pusieron un valor de 528 millones de francos por 32,99% de participación",
        "528000000 Franco");

    basicTest(it, model,
        "Rusia aceptó un préstamo del Banco Mundial de US$ 150 millones para combatir la propagación del sida y la tuberculosis, poniendo fin a un proceso de negociación que duró cuatro años, dijeron el viernes funcionarios del Banco Mundial.",
        "150000000 Dólar estadounidense");

    basicTest(it, model,
        "El pacto de la campana anterior estaba valorado en alrededor de $ 98 por acción",
        "98 Dólar");

    basicTest(it, model,
        "Un distribuidor dijo que la conversación fue que la firma vendió cerca de 500 millones de dólares de bonos de 30 años",
        "500000000 Dólar");

    basicTest(it, model,
        "Para el tercer trimestre, Sears dijo que sus ingresos totales aumentaron 4. 8% a $ 13180 millones a un año antes.",
        "13180000000 Dólar");

    basicTest(it, model,
        "Para los nueve meses, el etil dijo que la red cayó 2% o $ 1,40 por acción",
        "1,4 Dólar");

    basicTest(it, model,
        "Las expectativas de los analistas sugieren un déficit en cuenta corriente de septiembre de 1. 6 mil millones ($ 2,54 mil millones), comparado con los 2MM de agosto en déficit.",
        "2540000000 Dólar");

    basicTest(it, model,
        "125 millones de dólares australianos de eurobonos de cupón, a un precio de 50,9375 para producir 15,06% menos comisiones a través de Hambros Bank ltd.",
        "125000000 Dólar australiano");

    basicTest(it, model,
        "El viernes, el secretario jefe del gabinete anunció que ocho ministros del gabinete habían recibido cinco millones de yenes de la industria",
        "5000000 Yen");

    basicTest(it, model,
        "Incluyendo 450.000 yenes por el primer ministro Toshiki Kaifu",
        "450000 Yen");

    basicTest(it, model,
        "Dóllar : 143,80 yenes, arriba de 0,95; 1.8500 puntos, arriba de 0,0085.",
        "143,8 Yen");

    basicTest(it, model,
        "Orkem S.A., un fabricante francés de productos químicos controlados por el Estado, está haciendo una oferta amistosa de 470 peniques por acción para los 59,2% de UK",
        "470 Penique");

    basicTest(it, model,
        "Agosto, el gasto ajustado de las familias asalariadas disminuyó 0,6% a 309.381 yenes de un año antes.",
        "309381 Yen");

    basicTest(it, model,
        "Sr. Bowder dijo que los C$ 300 millones de ingresos...",
        "300000000 Dólar canadiense");

    basicTest(it, model,
        "Ascendería a alrededor de C$ 1,34 por acción.",
        "1,34 Dólar canadiense");

    basicTest(it, model,
        "Los precios de los huevos promediaron 64,2 centavos la docena.",
        "64,2 Centavo");

    basicTest(it, model,
        "Aún así, dijo que espera que las ventas para 1989 sean del orden de los 20.000 millones de francos, lo que refleja la facturación anticipada de dos grandes contratos en la segunda mitad del año.",
        "20000000000 Franco");

    basicTest(it, model,
        "La transacción pidió a Murdoch's News International, una unidad de noticias australia corp., para suscribir una emisión de derechos valorada en 6,65 mil millones de pesetas.",
        "6650000000 Peseta");

    basicTest(it, model,
        "Fujitsu ltd dijo que quiere retirar su polémica oferta de un yen para diseñar un sistema de computadoras de agua para la ciudad de hiroshima.",
        "1 Yen");

    basicTest(it, model,
        "250 millones de florines neerlandeses de 7 3/4% de bonos debidos nov. 15, 1999, a un precio de 101 1/4 para dar 7. 57% al precio de emisión y 7. 86% menos los honorarios completos, vía el banco del amro.",
        "250000000 Florín neerlandés");

    basicTest(it, model,
        "Además, el banco tiene la opción de comprar una participación de 30,84% en BIP societe generale después de enero. 1.1990 a 1.015 francos por acción.",
        "1015 Franco");

    basicTest(it, model,
        "Sus acciones se deslizaron en los últimos tratos para cerrar un centavo",
        "1 Centavo");

    basicTest(it, model,
        "Por acción menor a 197 peniques.",
        "197 Penique");

    basicTest(it, model,
        "Su beneficio operativo trimestral mejoró a 361 millones de libras",
        "361000000 Libra");

    basicTest(it, model,
        "El año pasado, el valor bruto de producción de las empresas del municipio de toda la ciudad se rompió por 100 millones de yuanes por primera vez, ocupando el primer lugar en toda la provincia.",
        "100000000 Yuan chino");

    basicTest(it, model,
        "Los guardabosques consiguieron guardar £ 50 millones ahorrados por el consejo de Baxendale-Walker.",
        "50000000 Libra");

    basicTest(it, model,
        "A su vez, francis leung pak-to ha acordado vender una participación de 8% en PCCW a telefónica por 323 millones de euros.",
        "323000000 Euro");

    basicTest(it, model,
        "La UEFA acusó a ferguson de desacreditar el juego con sus comentarios, y el 1 de mayo de ese año fue multado con 10.000 francos suizos.",
        "10000 Franco suizo");

    basicTest(it, model,
        "El IPL firmó a las líneas aéreas de martín pescador como el socio oficial del árbitro para la serie en un reparto (aproximadamente £ 15 millones).",
        "15000000 Libra");

    basicTest(it, model,
        "Los ingresos de la industria electrónica de adelaide ha crecido en alrededor del 15% anual desde 1990, y en 2011 supera los $ 4 mil millones.",
        "4000000000 Dólar");

    basicTest(it, model,
        "Abel y sus asociados ofrecen 4 millones de dólares por hacer los efectos de la película.",
        "4000000 Dólar");

    basicTest(it, model,
        "Malone demandó a 20th century-fox por $ 1,6 millones por incumplimiento de contrato.",
        "1600000 Dólar");

    basicTest(it, model,
        "En 2003, Bayern Munich prestó € 2 millones a Dortmund por un par de meses para pagar su nómina.",
        "2000000 Euro");

    basicTest(it, model,
        "Lockheed Martin y el gobierno de los Estados Unidos intensamente presionaron para el contrato de US$ 10 mil millones de la India para 126 aviones de combate.",
        "10000000000 Dólar estadounidense");

    basicTest(it, model,
        "Según la firma de investigación NPD, el precio de venta promedio de todas las PC portátiles de las ventanas ha caído de $ 659 en octubre de 2008 a",
        "659 Dólar");

    basicTest(it, model,
        "Tel flotó en la bolsa australiana a $ 2 por acción en noviembre de 1997.",
        "2 Dólar");

    basicTest(it, model,
        "El stand del este (la avenida de Worcester) se terminó en 1934 y esta capacidad aumentada a alrededor 80.000 espectadores pero costó £ 60.000.",
        "60000 Libra");

    basicTest(it, model,
        "Su compañero de equipo Fulham Johnny Haynes se convirtió en el primer jugador de £ 100.",
        "100 Libra");

    basicTest(it, model,
        "Para los nueve meses, la red de AMR subió 15% a $ 415,9 millones",
        "415900000 Dólar");

    basicTest(it, model,
        "El precio de la acción de la aerolínea ya está muy por debajo del nivel de 210 peniques visto después de que la compañía anunció la emisión de derechos a fines de septiembre.",
        "210 Penique");

    basicTest(it, model,
        "Rolling Stone observó, \"Harpercollins adquirió el proyecto de libro por $ 3 millones en 2008.",
        "3000000 Dólar");

    basicTest(it, model,
        "Su conclusión fue un pronunciamiento terso que $ 48 \"no es adecuado\"",
        "48 Dólar");

    basicTest(it, model,
        "2013, la edición de la revista forbes presenta a Keith en la portada con el título música country's $ 500 millones.",
        "500000000 Dólar");

    basicTest(it, model,
        "Harry Ferguson nos demandó por el uso ilegal de sus patentes pidiendo una indemnización de £ 90 millones, resuelto fuera de la corte en 1952.",
        "90000000 Libra");

    basicTest(it, model,
        "Aerosmith firmó con Columbia a mediados de 1972 por $ 125.000 y publicó su álbum debut, Aerosmith.",
        "125000 Dólar");

    basicTest(it, model,
        "Fue una de las mayores adquisiciones de Coke desde que compró Odwalla Inc. por $ 186 millones en 2001.",
        "186000000 Dólar");

    basicTest(it, model,
        "Apple y Creative llegaron a un acuerdo, con Apple pagando $ 100 millones a Creative y Creative para unirse al programa de accesorios \"hecho para ipod\".",
        "100000000 Dólar");

    basicTest(it, model,
        "A su vez, francis leung pak-a ha acordado vender una participación de 8% en PCCW a telefónica por 323 millones de euros.",
        "323000000 Euro");

    basicTest(it, model,
        "Malone demandó a 20th century-fox por 1,6 millones de dólares por incumplimiento de contrato;",
        "1600000 Dólar");

    basicTest(it, model,
        "En 2003, Bayern munich prestó € 2 millones a Dortmund por un par de meses para pagar su nómina.",
        "2000000 Euro");

    basicTest(it, model,
        "Lockheed martin y el gobierno de los estados unidos intensamente presionaron para el contrato de U$D 10 mil millones de la India para 126 aviones de combate.",
        "10000000000 Dólar estadounidense");

    multiTest(it, model,
        "La presentación de hart-scott se revisa y se resuelve cualquier problema antimonopolio. Por lo general, hart-scott se utiliza ahora para dar a los gerentes de las firmas objetivo noticias tempranas de una oferta y la oportunidad de utilizar la revisión regulatoria como una táctica de retraso. El impuesto de 20.000 dólares sería un pequeño costo en un acuerdo de varios billones de dólares, pero un grave obstáculo para miles de pequeños acuerdos amistosos.",
        ["20000 Dólar", "Dólar"]);

    multiTest(it, model,
        "El fideicomiso de rentas nacionales de bienes raíces dijo que reanudará los pagos de dividendos con un dividendo de 12 centavos de dólar que se pagará el 6 de noviembre a las acciones de récord el 25 de octubre.",
        ["12 Centavo", "Dólar"]);
});

describe('Temperature .', it => {
    let model = NumberWithUnitRecognizer.instance.getTemperatureModel(Culture.Spanish, false);

    basicTest(it, model,
        "La temperatura exterior es de 40 grados Celsius",
        "40 Grado Celsius");

    basicTest(it, model,
        "Hace 90 fahrenheit en Texas",
        "90 Grado Fahrenheit");

    multiTest(it, model,
        "Convertir 10 celsius a fahrenheit",
        ["10 Grado Celsius", "Grado Fahrenheit"]);

    basicTest(it, model,
        "-5 grados Fahrenheit",
        "-5 Grado Fahrenheit");

    basicTest(it, model,
        "6 grados centígrados",
        "6 Grado Celsius");

    basicTest(it, model,
        "98,6 grados f es temperatura normal",
        "98,6 Grado Fahrenheit");

    basicTest(it, model,
        "Ajuste la temperatura a 30 grados celsius",
        "30 Grado Celsius");

    basicTest(it, model,
        "La temperatura normal es 98,6 grados Fahrenheit",
        "98,6 Grado Fahrenheit");

    basicTest(it, model,
        "100 grados f",
        "100 Grado Fahrenheit");

    basicTest(it, model,
        "20 Grados c",
        "20 Grado Celsius");

    basicTest(it, model,
        "100 °f",
        "100 Grado Fahrenheit");

    basicTest(it, model,
        "20 °c",
        "20 Grado Celsius");

    basicTest(it, model,
        "100,2 Grados Fahrenheit es bajo",
        "100,2 Grado Fahrenheit");

    multiTest(it, model,
        "34,9 centígrado a fahrenheit",
        ["34,9 Grado Celsius", "Grado Fahrenheit"]);

    multiTest(it, model,
        "convertir 200 celsius celsius en fahrenheit",
        ["200 Grado Celsius", "Grado Celsius", "Grado Fahrenheit"]);

    multiTest(it, model,
        "convertir 200 K en fahrenheit",
        ["200 Kelvin", "Grado Fahrenheit"]);

    multiTest(it, model,
        "fahrenheit a celsius, cuantos celsius son 101 fahrenheit",
        ["101 Grado Fahrenheit", "Grado Fahrenheit", "Grado Celsius", "Grado Celsius"]);

    multiTest(it, model,
        "50 grados centígrados celsius a fahrenheit",
        ["50 Grado Celsius", "Grado Celsius", "Grado Fahrenheit"]);

    multiTest(it, model,
        "Podría convertir 51 fahrenheit en grados celsius",
        ["51 Grado Fahrenheit", "Grado Celsius"]);

    multiTest(it, model,
        "Convertir 106 grados Fahrenheit a grados centígrados",
        ["106 Grado Fahrenheit", "Grado Celsius"]);

    multiTest(it, model,
        "Convertir 106 K a grados centígrados",
        ["106 Kelvin", "Grado Celsius"]);

    multiTest(it, model,
        "Convertir 45 grados Fahrenheit a Celsius",
        ["45 Grado Fahrenheit", "Grado Celsius"]);

    multiTest(it, model,
        "Cómo convertir - 20 grados Fahrenheit a Celsius",
        ["-20 Grado Fahrenheit", "Grado Celsius"]);

    basicTest(it, model,
        "10,5 celsius",
        "10,5 Grado Celsius");

    basicTest(it, model,
        "20 grados celsius",
        "20 Grado Celsius");

    basicTest(it, model,
        "20,3 celsius",
        "20,3 Grado Celsius");

    basicTest(it, model,
        "34,5 celsius",
        "34,5 Grado Celsius");

    basicTest(it, model,
        "La temperatura exterior es de 98 grados",
        "98 Grado");

    basicTest(it, model,
        "Ajuste el termostato a 85 °",
        "85 Grado");

    basicTest(it, model,
        "Ajuste el termostato a 85°",
        "85 Grado");

    basicTest(it, model,
        "Aumentar la temperatura en 5 grados",
        "5 Grado");

    basicTest(it, model,
        "Ajuste la temperatura a 70 grados f",
        "70 Grado Fahrenheit");

    basicTest(it, model,
        "Aumentar la temperatura en 20 grados",
        "20 Grado");

    basicTest(it, model,
        "Ajuste la temperatura a 100 grados",
        "100 Grado");

    basicTest(it, model,
        "Ajuste la temperatura a 100 Kelvin",
        "100 Kelvin");

    basicTest(it, model,
        "Mantener la temperatura a 75 grados f",
        "75 Grado Fahrenheit");

    basicTest(it, model,
        "Deje que la temperatura esté a 40 centígrados",
        "40 Grado Celsius");

    basicTest(it, model,
        "Deje que la temperatura esté a 50 grados.",
        "50 Grado");
});

describe('Dimension .', it => {
    let model = NumberWithUnitRecognizer.instance.getDimensionModel(Culture.Spanish, false);

    basicTest(it, model,
        "75ml",
        "75 Mililitro");

    basicTest(it, model,
        "Su mayor inconveniente puede ser su espesor de 3 pulgadas, lo suficientemente grande como para que un consultor lo describa como \"clunky\".",
        "3 Pulgada");

    basicTest(it, model,
        "Un tornado rugió a través de un area de unas diez millas de largo allí, matando al menos a catorce personas y convirtiendo decenas de hogares en escombros",
        "10 Milla");

    basicTest(it, model,
        "Se necesita más de 10 1/2 millas de cable y alambre para conectar todo y 23 equipos",
        "10,5 Milla");

    basicTest(it, model,
        "El viaje de seis millas de mi hotel al aeropuerto que debería tardar 20 minutos, tardó más de tres horas.",
        "6 Milla");

    basicTest(it, model,
        "Es lo que 1) explica por qué somos como nosotros mismos en lugar de Bo Jackson; 2) advierte que es posible ahogarse en un lago que promedia dos pies de profundidad; y 3) predice que 10.000 monos colocados ante 10.000 pianos producirían 1.118 melodías publicitables del rock'n'roll.",
        "2 Pie");

    basicTest(it, model,
        "El 19 de mayo, la FDA comenzó a detener las setas chinas en latas de 68 onzas después de que más de 100 personas en Mississippi, Nueva York y Pennsylvania se enfermaron al comer hongos contaminados.",
        "68 Onza");

    basicTest(it, model,
        "El sr. Hulings se regodea que vendió todas sus acciones una semana antes de que el mercado se desplomara 190 puntos en oct. 13, y está utilizando el dinero para ayudar a comprar una granja de caballos de 45 acres.",
        "45 Acre");

    basicTest(it, model,
        "Bartlett había levantado paredes sin ventanas (ladrillo, enrejado, seto) de ocho a diez pies de alto, convirtiendo sus interiores en una sombra stygiana de un día.",
        "10 Pie");

    basicTest(it, model,
        "'La administración no quiere sorpresas', comenta Jack Zaves, quien, como director de servicios de combustible de American Airlines, compra unos 2.400 millones de galones de combustible para aviones al año.",
        "2400000000 Galón");

    basicTest(it, model,
        "Un refrigerador de agua de 10 galones había caído al suelo, empapando la alfombra roja.",
        "10 Galón");

    basicTest(it, model,
        "Cerca, seis delfines se divertirán en un acuario de agua salada de 1,5 millones de galones.",
        "1500000 Galón");

    basicTest(it, model,
        "Y este bebé tiene más de dos libras.",
        "2 Libra");

    basicTest(it, model,
        "No confío en las personas que no comen, dijo ms. Volokh, aunque ella misma dejó de comer el almuerzo hace unos años para bajar 25 libras.",
        "25 Libra");

    basicTest(it, model,
        "Shell, una subsidiaria del grupo real holandés, se le permitirá exportar 0,9 billones de pies cúbicos, y el Golfo, una unidad de olympia & york developments ltd. se permitirá exportar",
        "900000000000 Pie cúbico");

    basicTest(it, model,
        "Ejércitos Tigrean ahora están 200 millas al norte de Addis Ababa, amenazando la ciudad de éstos, que cortaría la capital de Mengistu desde el puerto de Assab, a través del cual todos los combustibles y otros suministros llegan a Addis Ababa.",
        "200 Milla");

    basicTest(it, model,
        "Dijo que una de las pc tomó un viaje de tres pies deslizándose por el suelo.",
        "3 Pie");

    basicTest(it, model,
        "El núcleo de sus propiedades es de 190.000 metros cuadrados de propiedad increíblemente caras en el distrito de Marunouchi, el centro financiero y de negocios de Tokyo, a menudo en broma llamada 'pueblo Mitsubishi'",
        "190000 Metro cuadrado");

    basicTest(it, model,
        "El satélite, construido por Hughes para la organización internacional de satélites de telecomunicaciones, forma parte de un contrato de 700 millones de dólares otorgado a Hughes en 1982 para desarrollar cinco satélites de tres toneladas.",
        "3 Tonelada");

    basicTest(it, model,
        "En un informe de 1996 sobre armas biológicas, el centro de estudios estratégicos e internacionales, una institución de investigación de políticas públicas en Washington, advirtió que era fácil para los posibles terroristas montar armas biológicas utilizando equipo comercial con una capacidad de 130 galones.",
        "130 Galón");

    basicTest(it, model,
        "La recopilación de datos del departamento de comercio del grupo de comercio mostró que las importaciones de Agosto, el segundo mayor mensual del año, subieron un 5% respecto de las 1.458.000 toneladas de julio, pero por debajo del máximo del año pasado en junio de 1988.",
        "1458000 Tonelada");

    basicTest(it, model,
        "El 1 de noviembre, Singh tiró a unos seis pies de la taza",
        "6 Pie");

    basicTest(it, model,
        "Una t.métrica es igual a 2.204,62 libras.",
        "2204,62 Libra");

    basicTest(it, model,
        "Por lo que cuando el cultivo de psyllium del año que viene se coseche en marzo, puede ser menor que las 16.000 toneladas métricas de los últimos años, justo en la cresta del boom del psyllium.",
        "16000 Tonelada métrica");

    basicTest(it, model,
        "El 486 es el descendiente de una larga serie de chips Intel que comenzó a dominar el mercado desde que IBM eligió el chip de 16 bits 8088 para su primera computadora personal.",
        "16 bit");

    basicTest(it, model,
        "El ''jiotto caspita'' puede funcionar a más de 188 millas por hora, dijo un portavoz de la compañía.",
        "188 Milla por hora");

    basicTest(it, model,
        "La marina de guerra ha instalado una zona de aterrizaje para helicópteros de apenas 100 metros en una sala de operaciones móvil, apenas en las cercanías de Bagdad.",
        "100 Metro");

    basicTest(it, model,
        "Caltrans planea añadir una segunda cubierta para autobuses y las flotas de autos por encima de la mediana de un tramo de 2,5 millas de la autopista Harbor, al sur de Los Ángeles, cerca del coliseo conmemorativo.",
        "2,5 Milla");

    basicTest(it, model,
        "En mi viaje de cuatro millas a la sede de la granja cada mañana, conduje por otras cuatro casas vacías.",
        "4 Milla");

    basicTest(it, model,
        "Fuimos insultados, dijo Langa desde el cuartel general católico griego, a unos 325 kilómetros al noroeste de Bucarest.",
        "325 Kilómetro");

    basicTest(it, model,
        "Rotich es un pequeño (5 pies",
        "5 Pie");

    basicTest(it, model,
        "4 pulgadas) de 28 años de edad que no comenzó a correr en serio hasta hace tres años y no había competido en el interior hasta este mes.",
        "4 Pulgada");

    basicTest(it, model,
        "Raceway park (Minnesota) en Shakopee es un óvalo pavimentado de 1/4 de milla.",
        "0,25 Milla");

    basicTest(it, model,
        "Castlecrag montaña está situado al sur del lago Moat, 1,6 km al oeste del monte Frink a lo largo de la misma línea de cresta.",
        "1,6 Kilómetro");

    basicTest(it, model,
        "Las colinas de Javadi se encuentran a unos 17 km de Ambur.",
        "17 Kilómetro");

    basicTest(it, model,
        "Después de rodear el lago Michigan cerca de la exposición durante dos horas, el comandante Hugo Eckener aterrizó la aeronave de 776 pies en el cercano aeropuerto Curtiss-Wright en Glenview.",
        "776 Pie");

    basicTest(it, model,
        "El intercambio con la carretera 35 y la carretera 115 a Lindsay y Peterborough (salida 436) se encuentra a 500 metros al este de la carretera Bennett.",
        "500 Metro");

    basicTest(it, model,
        "En 1995 Cannon introdujo la primera lente SLR comercialmente disponible con estabilización de imagen interna, 75 - 300 mm f / 4 - 5. 6 es usm.",
        "-300 Milímetro");

    basicTest(it, model,
        "Los aspectos más destacados de los proyectos de ley son: -- una restricción de la cantidad de bienes raíces que una familia puede poseer, a 660 metros cuadrados en las seis ciudades más grandes de la nación, pero más en ciudades pequeñas y áreas rurales.",
        "660 Metro cuadrado");

    basicTest(it, model,
        "El proyecto cuesta 46,8 millones de dólares, y está destinado a aumentar la capacidad de producción de la empresa en un 25% a 34.500 toneladas métricas de cátodos de cobre al año.",
        "34500 Tonelada métrica");

    basicTest(it, model,
        "La producción canadiense de lingotes de acero totalizó 291.890 toneladas métricas en la semana terminada el oct. 7, un 14,8% más que el total de la semana anterior, informó Statistics Canada, una agencia federal.",
        "291890 Tonelada métrica");

    basicTest(it, model,
        "Las panteras floridas viven en el hogar que se extiende por 190 km2.",
        "190 Kilómetro cuadrado");

    basicTest(it, model,
        "Un asteroide de una milla de ancho nos golpea, en promedio, sólo una vez cada trescientos mil años.",
        "1 Milla");

    basicTest(it, model,
        "Sin embargo, Premier incorporó el tren de potencia Nissan A12 (1.171 cc y 52 bhp) en lugar del motor Fiat original junto con una caja de cambios manual de Nissan.",
        "1171 Centímetro cúbico");

    multiTest(it, model,
        "En toda la industria, la producción de petróleo en este país se redujo en 500.000 barriles diarios a [] barriles en los primeros ocho meses de este año.",
        ["500000 Barril", "Barril"]);

    multiTest(it, model,
        "Sterling Armaments de Dagenham, Essex produjo un kit de conversión que comprende un nuevo barril de 7,62 mm, una revista, un extractor y un eyector para la venta comercial.",
        ["7,62 Milímetro", "Barril"]);
    /*
    basicTest(it, model,
    "Los precios al por mayor de la electricidad de California, que habían sido limitados a 250 dólares por megavatio hora en un mercado regulado, han alcanzado su pico bajo la desregulación a 1400 dólares por megavatio hora.",
    "250 ???");

    basicTest(it, model,
    "El primer vuelo en Hawker Horsley el 21 de diciembre de 1935, 950 caballos de fuerza (708 kilovatios)...",
    "950 ???");

    basicTest(it, model,
    "Muy arriba en el campanario, las enormes campanas de bronce, montadas sobre ruedas, oscilan locamente en 360 grados, comenzando y terminando, sorprendentemente, en la posición invertida o boca arriba.",
    "360 Grados");

    basicTest(it, model,
    "Un austro-húngaro B III se equipó experimentalmente con un motor de 119 kilovatios (160 hp) de Mercedes D.III.",
    "119 ???");
    */
});

describe('Age .', it => {
    let model = NumberWithUnitRecognizer.instance.getAgeModel(Culture.Spanish, false);

    basicTest(it, model,
        "Cuando tenía cinco años, hacía meriendas de mentira con mis muñecas.",
        "5 Año");

    basicTest(it, model,
        "Esta saga se remonta a casi diez años atrás.",
        "10 Año");

    basicTest(it, model,
        "¡Mi pelo ya está gris y sólo tengo 29 años!",
        "29 Año");

    basicTest(it, model,
        "Ahora cuenta noventa y cinco años: tiene una perspectiva de las cosas y tiene memoria.",
        "95 Año");

    basicTest(it, model,
        "La Gran Muralla china tiene más de 500 años y se extiende más de 5,000 millas.",
        "500 Año");

    basicTest(it, model,
        "Ya tiene 60 años, pues en principio nació el 8 de mayo de 1945.",
        "60 Año");

    basicTest(it, model,
        "Y al 25% no se les diagnostica hasta que tienen casi tres años.",
        "3 Año");

    basicTest(it, model,
        "¿Cuándo se va aplicar una presión seria para cumplir realmente esa promesa formulada hace un año?",
        "1 Año");

    basicTest(it, model,
        "La sublevación se produjo cuando yo era un bebé y tenía tan solo diez meses.",
        "10 Mes");

    basicTest(it, model,
        "La propuesta de la Comisión tiene ya 8 meses.",
        "8 Mes");

    basicTest(it, model,
        "A alrededor del 50% de ellos no se les diagnostica hasta los dieciocho meses de edad.",
        "18 Mes");

    basicTest(it, model,
        "Es posible, pero en 2006 mataron a 330 000 focas arpa y el 95% de ellas tenían menos de tres meses.",
        "3 Mes");

    basicTest(it, model,
        "Si seguimos adelante con la resolución en el período parcial de sesiones de diciembre, tendrá para entonces tres semanas de antigüedad.",
        "3 Semana");

    basicTest(it, model,
        "También pueden revocar su consentimiento hasta que el hijo haya cumplido 6 semanas de edad.",
        "6 Semana");

    basicTest(it, model,
        "Otras materias primas deberán utilizarse en un plazo de cinco días.",
        "5 Día");

    basicTest(it, model,
        "Para clubes de los demás países, una cuenta vencida por 90 días se considera morosa.",
        "90 Día");
    /* NOT SUPPORTED
    basicTest(it, model,
        "Barcelona tiene un año y medio de vida",
        "1,5 Año");
    */
});

function basicTest(it, model, source, value) {
    it(source, t => {
        t.not(model, null);
        let result = model.parse(source);
        t.is(result.length, 1);
        t.is(getValue(result[0].resolution["value"]) + " " + result[0].resolution["unit"], value);
    });
}

function multiTest(it, model, source, values) {
    it(source, t => {
        t.not(model, null);
        let result = model.parse(source);
        t.is(result.length, values.length);
        t.deepEqual(result.map(r => (getValue(r.resolution["value"]) + " " + r.resolution["unit"]).trim()), values);
    });
}

function getValue(v) {
    if(v === undefined || v === null) return '';
    return v;
}