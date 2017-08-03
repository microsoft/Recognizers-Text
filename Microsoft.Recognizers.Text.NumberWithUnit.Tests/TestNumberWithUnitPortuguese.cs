using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnitPortuguese
    {
        private void BasicTest(IModel model, string source, string value)
        {
            var resultStr = model.Parse(source);
            var resultJson = resultStr;
            Assert.AreEqual(1, resultJson.Count);
            Assert.AreEqual(value, resultJson.First().Resolution["value"] + " " + resultJson.First().Resolution["unit"]);
        }

        private void BasicTest(IModel model, string source, string[] values)
        {
            var results = model.Parse(source);
            Assert.AreEqual(values.Length, results.Count);
            var resultsValues = results.Select(x => GetStringValue(x)).ToArray();
            CollectionAssert.AreEqual(values, resultsValues);
        }

        private string GetStringValue(ModelResult source)
        {
            object value, unit;
            source.Resolution.TryGetValue(nameof(value), out value);
            source.Resolution.TryGetValue(nameof(unit), out unit);
            return $"{value} {unit}".Trim();
        }

        [TestMethod]
        public void TestCurrency()
        {
            var model = NumberWithUnitRecognizer.Instance.GetCurrencyModel(Culture.Portuguese);

            BasicTest(model,
            "Condado de Montgomery, md. - - $ 75 milhões de obligaciones generales, Serie b , bonos consolidados de mejoramiento público de 1989 , A través de un Manufacturers Hanover Trust co. group.",
            "75000000 Dólar");

            BasicTest(model,
            "Conglomerado finlandés nokia ( oy ab ) dijo que llegó a un acuerdo para comprar la compañía de cable holandés NKF kabel b.v. por 420 milhoes de marcos finlandeses",
            "420000000 Marco finlandês");

            BasicTest(model,
            "Nacional pagó a Siegel y Shuster $ 94.000 para cancelar todas las reclamaciones.",
            "94000 Dólar");

            BasicTest(model,
            "Servicios de dinámica general co., una unidad de Dinámica General corp., ganó un contrato del ejército de $ 48,2 milhoes para establecer facilidades del mantenimiento para los vehículos con seguimiento en Paquistán.",
            "48200000 Dólar");

            BasicTest(model,
            "El precio del segundo simulador oscila entre C$ 16,4 milhoes",
            "16400000 Dólar canadense");

            BasicTest(model,
            "Golar Gas Holding co., una subsidiaria de Gotaas-Larsen Shipping corp., ofreciendo $ 280 milhoes por las notas preferidas de la hipoteca de buques, vía los mercados de capitales de Merrill Linch.",
            "280000000 Dólar");

            BasicTest(model,
            "Bard/Ems tenía 1988 ventas de cerca de $ 14 milhões, según Birtcher.",
            "14000000 Dólar");

            BasicTest(model,
            "Los precios del acuerdo comienzan en $ 12.345.",
            "12345 Dólar");

            BasicTest(model,
            "solamente Batman ha acumulado mas de $247 milhoes en taquilla hasta la fecha, convirtiendola en la película con mejor recaudación de Warner Bros.",
            "247000000 Dólar");

            BasicTest(model,
            "El patrimonio neto de Coyle fue estimado en £ 8,10 milhões en Octuble del 2014.",
            "8100000 Libra");

            BasicTest(model,
            "Los ingresos netos por intereses cayeron un 27% en el trimestre a $ 254 milhões",
            "254000000 Dólar");

            BasicTest(model,
            "Un tribunal de apelaciones federal anuló una regulación de gas natural que había impedido que las compañías de gasoductos pasaran a los clientes un gasto de $ um bilhão en costos de contratos controversiales",
            "1000000000 Dólar");

            BasicTest(model,
            "El trimestre de 1988 también incluyó ganancias únicas por un total de aproximadamente $ 35 milhões.",
            "35000000 Dólar");

            BasicTest(model,
            "Y.J.Park y su familia subsistió durante cuatro años para comprar un pequeño apartamento aquí, pero se encontró que cuanto más cerca estaban de llegar a ahorrar los $ 40.000 que originalmente necesitaban, más subia el precio.",
            "40000 Dólar");

            BasicTest(model,
            "E. Robert Wallach fue sentenciado por un juez en Nueva York a seis años de prisión y una multa de $250.000 por su extorsión en el escándalo de Wedtech.",
            "250000 Dólar");

            BasicTest(model,
            "Un artículo publicado el miércoles en la encuesta económica de Oriente Medio revela que Irak pidió a sus clientes que paguen 50 centavos más por barril de petróleo sobre el precio oficial do petróleo a 1 de dezembro en una cuenta que no está bajo la supervisión de las naciones unidas.",
            "50 Centavo");

            BasicTest(model,
            "La división Chevrolet de General Motors Corp., reaccionando a las ventas lentas, dijo que ofrecerá rebajas de $ 800 en su Beretta 1990, la versión de dos puertas de su línea base de autos compactos.",
            "800 Dólar");

            BasicTest(model,
            "(El almacenista también tomó $ 125 milhões de bonos Junior SCI TV como pago parcial para los activos de TV).",
            "125000000 Dólar");

            BasicTest(model,
            "En el mercado nacional de venta libre, las acciones de Scimed cayeron 2,75 dólares.",
            "2,75 Dólar");

            BasicTest(model,
            "Al mismo tiempo, los inversionistas estiman que la reestructuración reduciría la factura anual de intereses en efectivo de la compañía en aproximadamente U$D 90 milhões.",
            "90000000 Dólar estadunidense");

            BasicTest(model,
            "Los gastos de capital en 1990 aumentarán ligeramente, dijo Mr.Marous, de un estimado de $ 470 milhões este año",
            "470000000 Dólar");

            BasicTest(model,
            "Shearson \"realmente solo tiene $ 300 milhões de capital\", dice el sr. Bowman de S&P.",
            "300000000 Dólar");

            BasicTest(model,
            "Puede ser directo (él quiere el dinero para alimento) o increíblemente enrevesado; su hermana está en este momento cerca de la muerte en Hoboken, ha perdido su cartera y tiene sólo $ 1,22 en cambio para poner un billete de autobús, y no le das la diferencia?",
            "1,22 Dólar");

            BasicTest(model,
            "El contrato de diciembre subió 1,20 centavos",
            "1,2 Centavo");

            BasicTest(model,
            "Walter Kirchberger, un analista de Painewebber inc., dijo que ofrecer a los interesados un precio más alto de $ 70 por acción es \"un método bastante efectivo de bloquear\" la oferta.",
            "70 Dólar");

            BasicTest(model,
            "Las ventas netas para el tercer trimestre de este año fueron de $ 14 milhões más que el año pasado.",
            "14000000 Dólar");

            BasicTest(model,
            "La compañía matriz del primer banco nacional de Chicago, con 48.000 milhões de dólares en activos, dijo que se reservó de absorber pérdidas en préstamos e inversiones en países con dificultades financieras.",
            "48000000000 Dólar");

            BasicTest(model,
            "Fluor Corp. dijo que se le adjudicó un contrato de $ 300 milhões para prestar servicios de ingeniería y gestión de la construcción en una mina de cobre en Irian Jaya, Indonesia, para una unidad de Freeport-McMoran Copper co.",
            "300000000 Dólar");

            BasicTest(model,
            "La bolsa americana dijo que un asiento fue vendido por $ 5.000 desde la venta anterior el viernes pasado.",
            "5000 Dólar");

            BasicTest(model,
            "Warner Communications Inc., que está siendo adquirida por Time Warner, ha presentado una demanda por violación de contrato de um bilhão de dólares contra Sony y dos productores.",
            "1000000000 Dólar");

            BasicTest(model,
            "En agosto, Asarco, a través de su subsidiaria Lac d'amiante Du Québec, vendió el interés restante de un tercio en una sociedad limitada minera de amianto en Canadá por $ 11,7 milhões.",
            "11700000 Dólar");

            BasicTest(model,
            "En 1988, las exportaciones de juguetes y juegos de producción nacional cayeron un 19% desde 1987 hasta alcanzar los 10050 milhões de dólares de Hong Kong.",
            "10050000000 Dólar de Hong Kong");

            BasicTest(model,
            "Las ventas del cuarto trimestre fiscal crecieron cerca de 18% a $ 1,17 bilhões en comparacion com o ano anterior.",
            "1170000000 Dólar");

            BasicTest(model,
            "Durante la primera hora de ayer, los precios cayeron hasta 1/4 de punto, o bajaron alrededor de $ 2,50 por cada monto nominal.",
            "2,5 Dólar");

            BasicTest(model,
            "New Jersey, por ejemplo, se le pidió que aceptara $ 300.000, pero se negó",
            "300000 Dólar");

            BasicTest(model,
            "Las ventas subieron 6,2% a $ 1,45 bilhões",
            "1450000000 Dólar");

            BasicTest(model,
            "A partir de ayer por la tarde, los reembolsos representaron menos del 15% de la posición total de efectivo de alrededor de $ 2 bilhões de los fondos de acciones de fidelidad.",
            "2000000000 Dólar");

            BasicTest(model,
            "Onvia. Com Inc., bajó 34 centavos",
            "34 Centavo");

            BasicTest(model,
            "El nuevo folleto dice que si la adquisición hubiera sido completada antes, las ganancias antes de impuestos \"habrían sido insuficientes para cubrir sus cargos fijos, incluyendo intereses sobre títulos de deuda\", por aproximadamente $ 62,7 milhões en el primer semestre de 1989.",
            "62700000 Dólar");

            BasicTest(model,
            "Filenet señaló que tenía efectivo y valores negociables por un total de $ 22,5 milhões en septiembre.",
            "22500000 Dólar");

            BasicTest(model,
            "Para los 20 restaurantes más caros de la ciudad, el precio de una cena aumentó a $ 63,45, también hubo un aumento del 8 por ciento.",
            "63,45 Dólar");

            BasicTest(model,
            "Trans Mundo Airlines Inc., ofreciendo billetes senior de 150 milhões de dólares, a través de Drexel Burnham.",
            "150000000 Dólar");

            BasicTest(model,
            "El fettuccine con champiñones portobello cuesta $ 8,50.",
            "8,5 Dólar");

            BasicTest(model,
            "El delivery de marzo terminó con un anticipo de 14,27 centavos",
            "14,27 Centavo");

            BasicTest(model,
            "En el tercer trimestre de 1988 fue de $ 75,3 milhões",
            "75300000 Dólar");

            BasicTest(model,
            "La confianza de los demandantes del protector del dalkon $ 2,38 bilhões fue establecida",
            "2380000000 Dólar");

            BasicTest(model,
            "Los términos de la oferta pusieron un valor de 528 milhões de francos por 32,99% de participación",
            "528000000 Franco");

            BasicTest(model,
            "Rusia aceptó un préstamo del Banco Mundial de US$ 150 milhões para combatir la propagación del sida y la tuberculosis, poniendo fin a un proceso de negociación que duró cuatro años, dijeron el viernes funcionarios del Banco Mundial.",
            "150000000 Dólar estadunidense");

            BasicTest(model,
            "El pacto de la campana anterior estaba valorado en alrededor de $ 98 por acción",
            "98 Dólar");

            BasicTest(model,
            "Un distribuidor dijo que la conversación fue que la firma vendió cerca de 500 milhões de dólares de bonos de 30 años",
            "500000000 Dólar");

            BasicTest(model,
            "Para el tercer trimestre, Sears dijo que sus ingresos totales aumentaron 4. 8% a $ 13180 milhões a un año antes.",
            "13180000000 Dólar");

            BasicTest(model,
            "Para los nueve meses, el etil dijo que la red cayó 2% o $ 1,40 por acción",
            "1,4 Dólar");

            BasicTest(model,
            "Las expectativas de los analistas sugieren un déficit en cuenta corriente de septiembre de 1. 6 bilhões ($ 2,54 bilhões), comparado con los 2MM de agosto en déficit.",
            "2540000000 Dólar");

            BasicTest(model,
            "125 milhões de dólares australianos de eurobonos de cupón, a un precio de 50,9375 para producir 15,06% menos comisiones a través de Hambros Bank ltd.",
            "125000000 Dólar australiano");

            BasicTest(model,
            "El viernes, el secretario jefe del gabinete anunció que ocho ministros del gabinete habían recibido cinco milhões de ienes de la industria",
            "5000000 Yen");

            BasicTest(model,
            "Incluyendo 450.000 yenes por el primer ministro Toshiki Kaifu",
            "450000 Yen");

            BasicTest(model,
            "Dóllar : 143,80 yenes, arriba de 0,95; 1.8500 pontos, arriba de 0,0085.",
            "143,8 Yen");

            BasicTest(model,
            "Orkem S.A., un fabricante francés de productos químicos controlados por el Estado, está haciendo una oferta amistosa de 470 penies por acción para los 59,2% de UK",
            "470 Pêni");

            BasicTest(model,
            "Agosto, el gasto ajustado de las familias asalariadas disminuyó 0,6% a 309.381 yenes de un año antes.",
            "309381 Yen");

            BasicTest(model,
            "Sr. Bowder dijo que los C$ 300 milhões de ingresos...",
            "300000000 Dólar canadense");

            BasicTest(model,
            "Ascendería a alrededor de C$ 1,34 por acción.",
            "1,34 Dólar canadense");

            BasicTest(model,
            "Los precios de los huevos promediaron 64,2 centavos la docena.",
            "64,2 Centavo");

            BasicTest(model,
            "Aún así, dijo que espera que las ventas para 1989 sean del orden de los 20.000 milhões de francos, lo que refleja la facturación anticipada de dos grandes contratos en la segunda mitad del año.",
            "20000000000 Franco");

            BasicTest(model,
            "La transacción pidió a Murdoch's News International, una unidad de noticias australia corp., para suscribir una emisión de derechos valorada en 6,65 bilhões de pesetas.",
            "6650000000 Peseta");

            BasicTest(model,
            "Fujitsu ltd dijo que quiere retirar su polémica oferta de um yen para diseñar un sistema de computadoras de agua para la ciudad de hiroshima.",
            "1 Yen");

            BasicTest(model,
            "250 milhões de florins holandeses de 7 3/4% de bonos debidos nov. 15, 1999, a un precio de 101 1/4 para dar 7. 57% do preço de emisión y 7. 86% menos los honorarios completos, vía el banco del amro.",
            "250000000 Florim holandês");

            BasicTest(model,
            "Además, el banco tiene la opción de comprar una participación de 30,84% en BIP societe generale después de enero. 1.1990 a 1.015 francos por acción.",
            "1015 Franco");

            BasicTest(model,
            "Sus acciones se deslizaron en los últimos tratos para cerrar um centavo",
            "1 Centavo");

            BasicTest(model,
            "Por acción menor a 197 penies.",
            "197 Pêni");

            BasicTest(model,
            "Su beneficio operativo trimestral mejoró a 361 milhões de libras",
            "361000000 Libra");

            BasicTest(model,
            "El año pasado, el valor bruto de producción de las empresas del municipio de toda la ciudad se rompió por 100 milhões de yuans por primera vez, ocupando el primer lugar en toda la provincia.",
            "100000000 Yuan chinês");

            BasicTest(model,
            "Los guardabosques consiguieron guardar £ 50 milhões ahorrados por el consejo de Baxendale-Walker.",
            "50000000 Libra");

            BasicTest(model,
            "A su vez, francis leung pak-to ha acordado vender una participación de 8% en PCCW a telefónica por 323 milhões de euros.",
            "323000000 Euro");

            BasicTest(model,
            "La UEFA acusó a ferguson de desacreditar el juego con sus comentarios, y el 1 de mayo de ese año fue multado con 10.000 francos suicos.",
            "10000 Franco suíço");

            BasicTest(model,
            "El IPL firmó a las líneas aéreas de martín pescador como el socio oficial del árbitro para la serie en un reparto (aproximadamente £ 15 milhões).",
            "15000000 Libra");

            BasicTest(model,
            "Los ingresos de la industria electrónica de adelaide ha crecido en alrededor del 15% anual desde 1990, y en 2011 supera os $ 4 bilhões.",
            "4000000000 Dólar");

            BasicTest(model,
            "Abel y sus asociados ofrecen 4 milhoes de dólares por hacer los efectos de la película.",
            "4000000 Dólar");

            BasicTest(model,
            "Malone demandó a 20th century-fox por $ 1,6 milhões por incumplimiento de contrato.",
            "1600000 Dólar");

            BasicTest(model,
            "En 2003, Bayern Munich prestó € 2 milhões a Dortmund por un par de meses para pagar su nómina.",
            "2000000 Euro");

            BasicTest(model,
            "Lockheed Martin y el gobierno de los Estados Unidos intensamente presionaron para el contrato de US$ 10 bilhões de la India para 126 aviones de combate.",
            "10000000000 Dólar estadunidense");

            BasicTest(model,
            "Según la firma de investigación NPD, el precio de venta promedio de todas las PC portátiles de las ventanas ha caído de $ 659 en octubre de 2008 a",
            "659 Dólar");

            BasicTest(model,
            "Tel flotó en la bolsa australiana a $ 2 por acción en noviembre de 1997.",
            "2 Dólar");

            BasicTest(model,
            "El stand del este (la avenida de Worcester) se terminó en 1934 y esta capacidad aumentada a alrededor 80.000 espectadores pero costó £ 60.000.",
            "60000 Libra");

            BasicTest(model,
            "Su compañero de equipo Fulham Johnny Haynes se convirtió en el primer jugador de £ 100.",
            "100 Libra");

            BasicTest(model,
            "Para los nueve meses, la red de AMR subió 15% a $ 415,9 milhões",
            "415900000 Dólar");

            BasicTest(model,
            "El precio de la acción de la aerolínea ya está muy por debajo del nivel de 210 penies visto después de que la compañía anunció la emisión de derechos a fines de septiembre.",
            "210 Pêni");

            BasicTest(model,
            "Rolling Stone observó, \"Harpercollins adquirió el proyecto de libro por $ 3 milhoes en 2008.",
            "3000000 Dólar");

            BasicTest(model,
            "Su conclusión fue un pronunciamiento terso que $ 48 \"no es adecuado\"",
            "48 Dólar");

            BasicTest(model,
            "2013, la edición de la revista forbes presenta a Keith en la portada con el título música country's $ 500 milhoes.",
            "500000000 Dólar");

            BasicTest(model,
            "Harry Ferguson nos demandó por el uso ilegal de sus patentes pidiendo una indemnización de £ 90 milhões, resuelto fuera de la corte en 1952.",
            "90000000 Libra");

            BasicTest(model,
            "Aerosmith firmó con Columbia a mediados de 1972 por $ 125.000 y publicó su álbum debut, Aerosmith.",
            "125000 Dólar");

            BasicTest(model,
            "Fue una de las mayores adquisiciones de Coke desde que compró Odwalla Inc. por $ 186 milhões en 2001.",
            "186000000 Dólar");

            BasicTest(model,
            "Apple y Creative llegaron a un acuerdo, con Apple pagando $ 100 milhões a Creative y Creative para unir-se ao programa de acesórios \"feito para ipod\".",
            "100000000 Dólar");

            BasicTest(model,
            "A su vez, francis leung pak-a ha acordado vender una participación de 8% en PCCW a telefónica por 323 milhões de euros.",
            "323000000 Euro");

            BasicTest(model,
            "Malone demandó a 20th century-fox por 1,6 milhoes de dólares por incumplimiento de contrato;",
            "1600000 Dólar");

            BasicTest(model,
            "En 2003, Bayern munich prestó € 2 milhões a Dortmund por un par de meses para pagar su nómina.",
            "2000000 Euro");

            BasicTest(model,
            "Lockheed martin y el gobierno de los estados unidos intensamente presionaron para el contrato de U$D 10 bilhões de la India para 126 aviones de combate.",
            "10000000000 Dólar estadunidense");

            BasicTest(model,
            "La presentación de hart-scott se revisa y se resuelve cualquier problema antimonopolio. Por lo general, hart-scott se utiliza ahora para dar a los gerentes de las firmas objetivo noticias tempranas de una oferta y la oportunidad de utilizar la revisión regulatoria como una táctica de retraso. El impuesto de 20.000 dólares sería un pequeño costo en un acuerdo de varios billones de dólares, pero un grave obstáculo para miles de pequeños acuerdos amistosos.",
            new string[] { "20000 Dólar", "Dólar" });

            BasicTest(model,
            "El fideicomiso de rentas nacionales de bienes raíces dijo que reanudará los pagos de dividendos con un dividendo de 12 centavos de dólar que se pagará el 6 de noviembre a las acciones de récord el 25 de octubre.",
            new string[] { "12 Centavo", "Dólar" });

        }

        [TestMethod]
        public void TestDimension()
        {
            var model = NumberWithUnitRecognizer.Instance.GetDimensionModel(Culture.Portuguese);

            BasicTest(model, "são 180,25ml liquidos", "180,25 Mililitro");

            BasicTest(model, "sao 180ml líquidos", "180 Mililitro");

            BasicTest(model, " 29km caminhando ", "29 Quilômetro");

            BasicTest(model, "são ,25ml liquidos", "0,25 Mililitro");

            BasicTest(model,
            "75ml",
            "75 Mililitro");

            BasicTest(model,
            "Su mayor inconveniente puede ser su espesor de 3 polegadas, lo suficientemente grande como para que un consultor lo describa como \"clunky\".",
            "3 Polegada");

            BasicTest(model,
            "Se necesita más de 10 1/2 milhas de cable y alambre para conectar todo y 23 equipos",
            "10,5 Milha");

            BasicTest(model,
            "Es lo que 1) explica por qué somos como nosotros mismos en lugar de Bo Jackson; 2) advierte que es posible ahogarse en un lago que promedia dois pés de profundidad; y 3) predice que 10.000 monos colocados ante 10.000 pianos producirían 1.118 melodías publicitables del rock'n'roll.",
            "2 Pé");

            BasicTest(model,
            "El sr. Hulings se regodea que vendió todas sus acciones una semana antes de que el mercado se desplomara 190 puntos en oct. 13, y está utilizando el dinero para ayudar a comprar una granja de caballos de 45 acres.",
            "45 Acre");

            BasicTest(model,
            "Bartlett había levantado paredes sin ventanas (ladrillo, enrejado, seto) de ocho a dez pés de altura, convirtiendo sus interiores en una sombra stygiana de un día.",
            "10 Pé");

            BasicTest(model,
            "'La administración no quiere sorpresas', comenta Jack Zaves, quien, como director de servicios de combustible de American Airlines, compra unos 2.400 milhões de galoes de combustible para aviones ao ano.",
            "2400000000 Galão");

            BasicTest(model,
            "Un refrigerador de agua de 10 galoes había caído no solo, empapando la alfombra roja.",
            "10 Galão");

            BasicTest(model,
            "Cerca, seis delfines se divertirán en un acuario de agua salada de 1,5 milhoes de galões.",
            "1500000 Galão");

            BasicTest(model,
            "Y este bebé tiene más de duas libras.",
            "2 Libra");

            BasicTest(model,
            "No confío en las personas que no comen, dijo ms. Volokh, aunque ella misma dejó de comer el almuerzo hace unos años para bajar 25 libras.",
            "25 Libra");

            BasicTest(model,
            "Un tornado rugió através de un area de umas dez milhas de área, matando ao menos a catorce personas y convirtiendo decenas de hogares en escombros",
            "10 Milha");

            BasicTest(model,
            "Shell, una subsidiaria del grupo real holandés, se le permitirá exportar 0,9 bilhões de pés cúbicos, y el Golfo, una unidad de olympia & york developments ltd. se permitirá exportar",
            "900000000 Pé cúbico");

            BasicTest(model,
            "Ejércitos Tigrean ahora están 200 milhas ao norte de Addis Ababa, amenazando la ciudad de éstos, que cortaría la capital de Mengistu desde el puerto de Assab, a través del cual todos los combustibles y otros suministros llegan a Addis Ababa.",
            "200 Milha");

            BasicTest(model,
            "Dijo que una de las pc tomó un viaje de tres pes deslizándose por el suelo.",
            "3 Pé");

            BasicTest(model,
            "El núcleo de sus propiedades es de 190.000 metros quadrados de propiedad increíblemente caras en el distrito de Marunouchi, el centro financiero y de negocios de Tokyo, a menudo en broma llamada 'pueblo Mitsubishi'",
            "190000 Metro quadrado");

            BasicTest(model,
            "El satélite, construido por Hughes para la organización internacional de satélites de telecomunicaciones, forma parte de un contrato de 700 millones de dólares otorgado a Hughes en 1982 para desarrollar cinco satélites de tres toneladas.",
            "3 Tonelada");

            BasicTest(model,
            "En un informe de 1996 sobre armas biológicas, el centro de estudios estratégicos e internacionales, una institución de investigación de políticas públicas en Washington, advirtió que era fácil para los posibles terroristas montar armas biológicas utilizando equipo comercial con una capacidad de 130 galoes.",
            "130 Galão");

            BasicTest(model,
            "La recopilación de datos del departamento de comercio del grupo de comercio mostró que las importaciones de Agosto, el segundo mayor mensual del año, subieron un 5% respecto de las 1.458.000 toneladas de julio, pero por debajo del máximo del año pasado en junio de 1988.",
            "1458000 Tonelada");

            BasicTest(model,
            "El 1 de noviembre, Singh tiró a unos seis pés de la taza",
            "6 Pé");

            BasicTest(model,
            "Una t.métrica es igual a 2.204,62 libras.",
            "2204,62 Libra");

            BasicTest(model,
            "Por lo que cuando el cultivo de psyllium del año que viene se coseche en marzo, puede ser menor que las 16.000 toneladas métricas de los últimos años, justo en la cresta del boom del psyllium.",
            "16000 Tonelada métrica");

            BasicTest(model,
            "El 486 es el descendiente de una larga serie de chips Intel que comenzó a dominar el mercado desde que IBM eligió el chip de 16 bits 8088 para su primera computadora personal.",
            "16 bit");

            BasicTest(model,
            "El ''jiotto caspita'' puede funcionar a más de 188 milhas por hora, dijo un portavoz de la compañía.",
            "188 Milha por hora");

            BasicTest(model,
            "La marina de guerra ha instalado una zona de aterrizaje para helicópteros de apenas 100 metros en una sala de operaciones móvil, apenas en las cercanías de Bagdad.",
            "100 Metro");

            BasicTest(model,
            "Caltrans planea añadir una segunda cubierta para autobuses y las flotas de autos por encima de la mediana de un tramo de 2,5 milhas de la autopista Harbor, ao sul de Los Ángeles, cerca del coliseo conmemorativo.",
            "2,5 Milha");

            BasicTest(model,
            "Em minha viaje de quatro milhas a la sede de la granja cada mañana, conduje por otras quatro casas vacías.",
            "4 Milha");

            BasicTest(model,
            "Fuimos insultados, dijo Langa desde el cuartel general católico griego, a unos 325 quilometro ao noroeste de Bucarest.",
            "325 Quilômetro");

            BasicTest(model,
            "Rotich es un pequeño (5 pés",
            "5 Pé");

            BasicTest(model,
            "4 polegadas) de 28 años de edad que no comenzó a correr en serio hasta hace tres años y no había competido en el interior hasta este mes.",
            "4 Polegada");

            BasicTest(model,
            "Raceway park (Minnesota) en Shakopee es un óvalo pavimentado de 1/4 de milha.",
            "0,25 Milha");

            BasicTest(model,
            "Castlecrag montaña está situado ao sul do lago Moat, 1,6 km ao oeste del monte Frink a lo largo de la misma línea de cresta.",
            "1,6 Quilômetro");

            BasicTest(model,
            "Las colinas de Javadi se encuentran a unos 17 km de Ambur.",
            "17 Quilômetro");

            BasicTest(model,
            "Después de rodear el lago Michigan cerca de la exposición durante dos horas, el comandante Hugo Eckener aterrizó la aeronave de 776 pes en el cercano aeropuerto Curtiss-Wright en Glenview.",
            "776 Pé");

            BasicTest(model,
            "El intercambio con la carretera 35 y la carretera 115 a Lindsay y Peterborough (salida 436) se encuentra a 500 metros ao leste de la carretera Bennett.",
            "500 Metro");

            BasicTest(model,
            "En 1995 Cannon introdujo la primera lente SLR comercialmente disponible con estabilización de imagen interna, 75 - 300 mm f / 4 - 5. 6 es usm.",
            "-300 Milímetro");

            BasicTest(model,
            "Los aspectos más destacados de los proyectos de ley son: -- una restricción de la cantidad de bienes raíces que una familia puede poseer, a 660 metros quadrados en las seis ciudades más grandes de la nación, pero más en ciudades pequeñas y áreas rurales.",
            "660 Metro quadrado");

            BasicTest(model,
            "El proyecto cuesta 46,8 millones de dólares, y está destinado a aumentar la capacidad de producción de la empresa en un 25% a 34.500 toneladas métricas de cátodos de cobre ao ano.",
            "34500 Tonelada métrica");

            BasicTest(model,
            "La producción canadiense de lingotes de acero totalizó 291.890 toneladas métricas en la semana terminada el oct. 7, un 14,8% más que el total de la semana anterior, informó Statistics Canada, una agencia federal.",
            "291890 Tonelada métrica");

            BasicTest(model,
            "Las panteras floridas viven en el hogar que se extiende por 190 km2.",
            "190 Quilômetro quadrado");

            BasicTest(model,
            "Un asteroide de uma milha de ancho nos golpea, en promedio, sólo una vez cada trescientos mil años.",
            "1 Milha");

            BasicTest(model,
            "Sin embargo, Premier incorporó el tren de potencia Nissan A12 (1.171 cc y 52 bhp) en lugar del motor Fiat original junto con una caja de cambios manual de Nissan.",
            "1171 Centímetro cúbico");

            BasicTest(model,
            "En toda la industria, la producción de petróleo en este país se redujo en 500.000 barris diarios a [] barris nos primeiros oito meses deste año.",
            new string[] { "500000 Barril", "Barril" });

            BasicTest(model,
            "Sterling Armaments de Dagenham, Essex produjo un kit de conversión que comprende un nuevo barril de 7,62 mm, una revista, un extractor y un eyector para la venta comercial.",
            new string[] { "7,62 Milímetro", "Barril" });

            BasicTest(model,
            "El 19 de mayo, la FDA comenzó a detener las setas chinas en latas de 68 oncas después de que más de 100 personas en Mississippi, Nueva York y Pennsylvania se enfermaron a comer hongos contaminados.",
            "68 Onça");

            BasicTest(model,
            "El viaje de seis milhas de meu hotel ao aeropuerto que debería tardar 20 minutos, tardó más de tres horas.",
            "6 Milha");

            /*
            BasicTest(model,
            "Los precios al por mayor de la electricidad de California, que habían sido limitados a 250 dólares por megavatio hora en un mercado regulado, han alcanzado su pico bajo la desregulación a 1400 dólares por megavatio hora.",
            "250 ???");

            BasicTest(model,
            "El primer vuelo en Hawker Horsley el 21 de diciembre de 1935, 950 caballos de fuerza (708 kilovatios)...",
            "950 ???");

            BasicTest(model,
            "Muy arriba en el campanario, las enormes campanas de bronce, montadas sobre ruedas, oscilan locamente en 360 grados, comenzando y terminando, sorprendentemente, en la posición invertida o boca arriba.",
            "360 Grados");

            BasicTest(model,
            "Un austro-húngaro B III se equipó experimentalmente con un motor de 119 kilovatios (160 hp) de Mercedes D.III.",
            "119 ???");
            */
        }

        [TestMethod]
        public void TestTemperature()
        {
            var model = NumberWithUnitRecognizer.Instance.GetTemperatureModel(Culture.Portuguese);

            BasicTest(model,
                "A temperatura externa é de 40 graus Celsius",
                "40 Grau Celsius");

            BasicTest(model,
                "Faz 90 fahrenheit no Texas",
                "90 Grau Fahrenheit");

            BasicTest(model,
                "Converter 10 celsius em fahrenheit",
                new string[] { "10 Grau Celsius", "Grau Fahrenheit" });

            BasicTest(model,
                "-5 graus Fahrenheit",
                "-5 Grau Fahrenheit");

            BasicTest(model,
                "6 graus centígrados",
                "6 Grau Celsius");

            BasicTest(model,
                "98,6 graus f é uma temperatura normal",
                "98,6 Grau Fahrenheit");

            BasicTest(model,
                "Ajuste a temperatura para 30 graus celsius",
                "30 Grau Celsius");

            BasicTest(model,
                "A temperatura normal é 98,6 graus Fahrenheit",
                "98,6 Grau Fahrenheit");

            BasicTest(model,
                "100 graus f",
                "100 Grau Fahrenheit");

            BasicTest(model,
                "20 Graus c",
                "20 Grau Celsius");

            BasicTest(model,
                "100 °f",
                "100 Grau Fahrenheit");

            BasicTest(model,
                "20 °c",
                "20 Grau Celsius");

            BasicTest(model,
                "100,2 Graus Fahrenheit é baixo",
                "100,2 Grau Fahrenheit");

            BasicTest(model,
                "34,9 centígrado pra fahrenheit",
                new string[] { "34,9 Grau Celsius", "Grau Fahrenheit" });

            BasicTest(model,
                "converter 200 celsius em fahrenheit",
                new string[] { "200 Grau Celsius", "Grau Fahrenheit" });

            BasicTest(model,
                "convertir 200 K em fahrenheit",
                new string[] { "200 Kelvin", "Grau Fahrenheit" });

            BasicTest(model,
                "fahrenheit pra celsius, quantos celsius são 101 fahrenheit",
                new string[] { "101 Grau Fahrenheit", "Grau Fahrenheit", "Grau Celsius", "Grau Celsius" });

            BasicTest(model,
                "50 graus centígrados celsius em fahrenheit",
                new string[] { "50 Grau Celsius", "Grau Celsius", "Grau Fahrenheit" });

            BasicTest(model,
                "Poderias converter 51 fahrenheit em graus celsius",
                new string[] { "51 Grau Fahrenheit", "Grau Celsius" });

            BasicTest(model,
                "Converter 106 graus Fahrenheit em graus centígrados",
                new string[] { "106 Grau Fahrenheit", "Grau Celsius" });

            BasicTest(model,
                "Converter 106 K em graus centígrados",
                new string[] { "106 Kelvin", "Grau Celsius" });

            BasicTest(model,
                "Converter 45 graus Fahrenheit a Celsius",
                new string[] { "45 Grau Fahrenheit", "Grau Celsius" });

            BasicTest(model,
                "Como convertir - 20 graus Fahrenheit para Celsius",
                new string[] { "-20 Grau Fahrenheit", "Grau Celsius" });

            BasicTest(model,
                "10,5 celsius",
                "10,5 Grau Celsius");

            BasicTest(model,
                "20 graus celsius",
                "20 Grau Celsius");

            BasicTest(model,
                "20,3 celsius",
                "20,3 Grau Celsius");

            BasicTest(model,
                "34,5 celsius",
                "34,5 Grau Celsius");

            BasicTest(model,
                "A temperatura exterior é de 98 graus",
                "98 Grau");

            BasicTest(model,
                "Ajuste o termostato em 85 °",
                "85 Grau");

            BasicTest(model,
                "Ajuste o termostato em 85°",
                "85 Grau");

            BasicTest(model,
                "Aumente a temperatura em 5 graus",
                "5 Grau");

            BasicTest(model,
                "Ajuste a temperatura para 70 graus f",
                "70 Grau Fahrenheit");

            BasicTest(model,
                "Aumentar a temperatura em 20 grau",
                "20 Grau");

            BasicTest(model,
                "Ajuste a temperatura a 100 graus",
                "100 Grau");

            BasicTest(model,
                "Ajuste a temperatura a 100 Kelvin",
                "100 Kelvin");

            BasicTest(model,
                "Mantenha a temperatura de 75 graus f",
                "75 Grau Fahrenheit");

            BasicTest(model,
                "Deixe que a temperatura fique em 40 centígrados",
                "40 Grau Celsius");

            BasicTest(model,
                "Deixe a temperatura em 50 graus.",
                "50 Grau");

            /* Not supported yet
            BasicTest(model,
                "menos 10 celsius",
                "-10 Grau Celsius");

            BasicTest(model,
                "A temperatura exterior é de menos 98 graus",
                "-98 Grau");

            BasicTest(model,
                "dez graus celsius negativos",
                "-10 Grau Celsius");

            BasicTest(model,
                "A temperatura exterior é de 98 graus negativos",
                "-98 Grau");
            */

        }

        [TestMethod]
        public void TestAge()
        {
            var model = NumberWithUnitRecognizer.Instance.GetAgeModel(Culture.Portuguese);

            BasicTest(model,
                "Quando tinha cinco anos, aprendeu a andar de bicicleta.",
                "5 Ano");

            BasicTest(model,
                "Esta saga remonta a quase dez anos atrás.",
                "10 Ano");

            BasicTest(model,
                "Só tenho 29 anos!",
                "29 Ano");

            BasicTest(model,
                "Agora com noventa e cinco anos tens perspectiva das coisas.",
                "95 Ano");

            BasicTest(model,
                "A Grande Muralha da China tem mais de 500 anos e se extende por mais de 5,000 milhas.",
                "500 Ano");

            BasicTest(model,
                "Já tem 60 anos, pois nasceu em 8 de maio de 1945.",
                "60 Ano");

            BasicTest(model,
                "25% dos casos não são diagnosticados até por volta dos tres anos.",
                "3 Ano");

            BasicTest(model,
                "Quando haverá pressão para comprir essa promessa feita há um ano?",
                "1 Ano");

            BasicTest(model,
                "Aconteceu quando era um bebê e tinha apenas dez meses.",
                "10 Mês");

            BasicTest(model,
                "A proposta da comissão já tem 8 meses de idade.",
                "8 Mês");

            BasicTest(model,
                "Aproximadamente 50% dos casos são diagnosticados aos dezoito meses de idade.",
                "18 Mês");

            BasicTest(model,
                "É possível, mas em 2006 95% delas tinham menos de tres meses de vida.",
                "3 Mês");

            BasicTest(model,
                "Se seguirmos adiante no período de dezembro, terão tres semanas de existência.",
                "3 Semana");

            BasicTest(model,
                "Às 6 semanas de idade já comemora o Natal.",
                "6 Semana");

            BasicTest(model,
                "Outras matérias primas devem ser usadas num prazo de cinco dias.",
                "5 Dia");

            BasicTest(model,
                "Uma conta vencida a 90 dias está bem atrasada.",
                "90 Dia");
            
            /* NOT SUPPORTED
            BasicTest(model,
                "Barcelona tem un ano e meio de vida",
                "1,5 Ano");
            */
        }
    }
}
