var NumberWithUnitRecognizer = require('../compiled/numberWithUnit/numberWithUnitRecognizer').default;
var Culture = require('../compiled/culture').Culture;
var describe = require('ava-spec').describe;

describe('Currency .', it => {
    let model = NumberWithUnitRecognizer.instance.getCurrencyModel(Culture.English, false);

    basicTest(it, model,
        "montgomery county , md . - - $ 75 million of general obligation , series b , consolidated public improvement bonds of 1989 , through a manufacturers hanover trust co . group .",
        "75000000 Dollar");

    basicTest(it, model,
        "finnish conglomerate nokia oy ab said it reached an agreement to buy dutch cable company nkf kabel b . v . for 420 million finnish markka .",
        "420000000 Finnish markka");

    basicTest(it, model,
        "national paid siegel and shuster $94,000 to drop all claims .",
        "94000 Dollar");

    basicTest(it, model,
        "general dynamics services co . , a unit of general dynamics corp . , won a $48.2 million army contract to establish maintenance facilities for tracked vehicles in pakistan .",
        "48200000 Dollar");

    basicTest(it, model,
        "the price of the second simulator ranges between c $ 16.4 million",
        "16400000 Canadian dollar");

    basicTest(it, model,
        "golar gas holding co . , a subsidiary of gotaas - larsen shipping corp . , offering of $ 280 million first preferred ship mortgage notes , via merrill lynch capital markets .",
        "280000000 Dollar");

    basicTest(it, model,
        "bard / ems had 1988 sales of about $ 14 million , birtcher said .",
        "14000000 Dollar");

    basicTest(it, model,
        "accord prices start at $ 12,345 .",
        "12345 Dollar");

    basicTest(it, model,
        "` ` batman ' ' alone has racked up more than $ 247 million in box - office receipts to date , making it warner bros . ' largest grossing film ever .",
        "247000000 Dollar");

    basicTest(it, model,
        "coyle ' s net worth was estimated at £ 8.10 million in october 2014 .",
        "8100000 Pound");

    basicTest(it, model,
        "net interest income sank 27 % in the quarter to $ 254 million.",
        "254000000 Dollar");

    basicTest(it, model,
        "a federal appeals court struck down a natural - gas regulation that had prevented pipeline companies from passing to customers part of $ 1 billion in costs from controversial ` ` take - or - pay ' ' contracts .",
        "1000000000 Dollar");

    basicTest(it, model,
        "the 1988 quarter also included one - time gains totaling about $ 35 million .",
        "35000000 Dollar");

    basicTest(it, model,
        "y . j . park and her family scrimped for four years to buy a tiny apartment here , but found that the closer they got to saving the $ 40,000 they originally needed , the more the price rose .",
        "40000 Dollar");

    basicTest(it, model,
        "e . robert wallach was sentenced by a u . s . judge in new york to six years in prison and fined $ 250,000 for his racketeering conviction in the wedtech scandal .",
        "250000 Dollar");

    basicTest(it, model,
        "an article in the middle east economic survey ( mees ) published today wednesday reveals that iraq has asked its clients to pay 50 cents more per barrel of oil over the official oil price as of december 1 into an account not under united nations supervision .",
        "50 Cent");

    basicTest(it, model,
        "general motors corp . ' s chevrolet division , reacting to slow sales , said it will offer $ 800 rebates on its 1990 beretta , the two - door version of its core compact - car line .",
        "800 Dollar");

    basicTest(it, model,
        "( storer also took $ 125 million of junior sci tv bonds as partial payment for the tv assets . )",
        "125000000 Dollar");

    basicTest(it, model,
        "in national over - the - counter trading friday , scimed shares tumbled $ 2.75 .",
        "2.75 Dollar");

    basicTest(it, model,
        "at the same time , investors estimate the restructuring would cut the company ' s annual cash interest bill from about $ 90 million.",
        "90000000 Dollar");

    basicTest(it, model,
        "capital expenditure in 1990 will rise slightly , mr . marous said , from an estimated $ 470 million this year .",
        "470000000 Dollar");

    basicTest(it, model,
        "shearson ` ` really only has $ 300 million of capital , ' ' says mr . bowman of s & p .",
        "300000000 Dollar");

    basicTest(it, model,
        "it may be straightforward - - he wants money for food - - or incredibly convoluted ; his sister is at this very moment near death in hoboken , he has lost his wallet and has only $ 1.22 in change to put toward a bus ticket costing , and wo n ' t you give him the difference ?",
        "1.22 Dollar");

    basicTest(it, model,
        "the december contract rose 1.20 cents ",
        "1.2 Cent");

    basicTest(it, model,
        "walter kirchberger , an analyst with painewebber inc . , said that offering holders a higher , $ 70 - a - share price is ` ` a fairly effective method of blocking ' ' the stena - tiphook bid .",
        "70 Dollar");

    basicTest(it, model,
        "net sales for this year ' s third quarter were $ 14 million  last year .",
        "14000000 Dollar");

    basicTest(it, model,
        "the parent company of first national bank of chicago , with $ 48 billion in assets , said it set aside to absorb losses on loans and investments in financially troubled countries .",
        "48000000000 Dollar");

    basicTest(it, model,
        "fluor corp . said it was awarded a $ 300 million contract to provide engineering and construction - management services at a copper mine in irian jaya , indonesia , for a unit of freeport - mcmoran copper co .",
        "300000000 Dollar");

    basicTest(it, model,
        "the american stock exchange said a seat was sold for down $ 5,000 from the previous sale last friday .",
        "5000 Dollar");

    basicTest(it, model,
        "warner communications inc . , which is being acquired by time warner , has filed a $ 1 billion breach - of - contract suit against sony and the two producers .",
        "1000000000 Dollar");

    basicTest(it, model,
        "in august , asarco , through its lac d ' amiante du quebec subsidiary , sold its remaining one - third interest in an asbestos mining limited partnership in canada for $ 11.7 million .",
        "11700000 Dollar");

    basicTest(it, model,
        "in 1988 , exports of domestically produced toys and games fell 19 % from 1987 , to hk $ 10.05 billion .",
        "10050000000 Hong Kong dollar");

    basicTest(it, model,
        "fiscal fourth - quarter sales grew about 18 % to from $ 1.17 billion a year earlier .",
        "1170000000 Dollar");

    basicTest(it, model,
        "during the first hour of trading yesterday , prices fell as much as 1 / 4 point , or down about $ 2.50 for each face amount .",
        "2.5 Dollar");

    basicTest(it, model,
        "new jersey , for example , was asked to accept $ 300,000 , but refused .",
        "300000 Dollar");

    basicTest(it, model,
        "sales rose 6 . 2 % to $ 1.45 billion .",
        "1450000000 Dollar");

    basicTest(it, model,
        "as of yesterday afternoon , the redemptions represented less than 15 % of the total cash position of about $ 2 billion of fidelity ' s stock funds .",
        "2000000000 Dollar");

    basicTest(it, model,
        "onvia . com inc . , down 34 cents",
        "34 Cent");

    basicTest(it, model,
        "the tw prospectus says that if the acquisition had been completed earlier , pretax earnings ` ` would have been insufficient to cover its fixed charges , including interest on debt securities , ' ' by approximately $ 62.7 million in the first six months of 1989 .",
        "62700000 Dollar");

    basicTest(it, model,
        "filenet noted that it had cash and marketable securities totaling $ 22.5 million on sept . 30 , and stockholders.",
        "22500000 Dollar");

    basicTest(it, model,
        "for the 20 most expensive restaurants in the city , the price of a dinner rose from $ 63.45 , also an 8 percent increase .",
        "63.45 Dollar");

    basicTest(it, model,
        "trans world airlines inc . , offering of $ 150 million senior notes , via drexel burnham .",
        "150000000 Dollar");

    basicTest(it, model,
        "the fettuccine with portobello mushrooms costs $ 8.50 .",
        "8.5 Dollar");

    basicTest(it, model,
        "the march delivery ended with an advance of  to 14.27 cents .",
        "14.27 Cent");

    basicTest(it, model,
        "interest expense in the 1988 third quarter was $ 75.3 million .",
        "75300000 Dollar");

    basicTest(it, model,
        "the $ 2.38 billion dalkon shield claimants trust was established as part of a . h . robins ' bankruptcy - reorganization plan to resolve injury claims arising from use of the shield .",
        "2380000000 Dollar");

    basicTest(it, model,
        "the terms of the offer put a value of 528 million francs on the 32 . 99 % shareholding .",
        "528000000 Franc");

    basicTest(it, model,
        "russia has accepted a us $ 150 million world bank loan to combat the spread of aids and tuberculosis , ending a negotiating process that lasted four years , world bank officials said friday .",
        "150000000 United States dollar");

    basicTest(it, model,
        "the previous bellsouth pact was valued at about $ 98 a share .",
        "98 Dollar");

    basicTest(it, model,
        "one dealer said the talk was that the firm sold about $ 500 million of bellwether 30 - year bonds .",
        "500000000 Dollar");

    basicTest(it, model,
        "for the third quarter , sears said its total revenue rose 4 . 8 % to $ 13.18 billion a year earlier .",
        "13180000000 Dollar");

    basicTest(it, model,
        "for the nine months , ethyl said net fell 2 % or $ 1.40 a share",
        "1.4 Dollar");

    basicTest(it, model,
        "analysts ' expectations suggest a september current account deficit of 1 . 6 billion ( $ 2.54 billion ) , compared with august ' s 2 . 0 billion deficit .",
        "2540000000 Dollar");

    basicTest(it, model,
        "125 million australian dollars of zero - coupon eurobonds due dec . 12 , 1994 , priced at 50 . 9375 to yield 15 . 06 % less fees via hambros bank ltd .",
        "125000000 Australian dollar");

    basicTest(it, model,
        "on friday , the chief cabinet secretary announced that eight cabinet ministers had received five million yen from the industry",
        "5000000 Japanese yen");

    basicTest(it, model,
        " including 450,000 yen by prime minister toshiki kaifu .",
        "450000 Japanese yen");

    basicTest(it, model,
        "orkem s . a . , a french state - controlled chemical manufacturer , is making a friendly bid of 470 pence a share for the 59 . 2 % of u . k . specialty chemical group coates brothers plc which it does n ' t already own , the two sides said .",
        "470 Pence");

    basicTest(it, model,
        "august adjusted spending by wage - earning families was down 0 . 6 % to 309,381 yen from a year earlier .",
        "309381 Japanese yen");

    basicTest(it, model,
        "national income realty trust said it will resume dividend payments with a 12-cent-a-share dividend to be paid nov . 6 to shares of record oct . 25 .",
        "12 Cent");

    basicTest(it, model,
        "mr . bowder said the c $ 300 million charge to earnings",
        "300000000 Canadian dollar");

    basicTest(it, model,
        "would amount to about c $ 1.34 a share .",
        "1.34 Canadian dollar");

    basicTest(it, model,
        "egg prices averaged 64.2 cents a dozen .",
        "64.2 Cent");

    basicTest(it, model,
        "still , it said it expects sales for all of 1989 to be on the order of 20 billion francs , reflecting anticipated billings for two large contracts in the second half of the year .",
        "20000000000 Franc");

    basicTest(it, model,
        "the transaction called for mr . murdoch ' s news international plc , a unit of australia - based news corp . , to subscribe to a rights issue by zeta valued at 6.65 billion pesetas .",
        "6650000000 Peseta");

    basicTest(it, model,
        "fujitsu ltd . said it wants to withdraw its controversial one-yen bid to design a waterworks computer system for the city of hiroshima .",
        "1 Japanese yen");

    basicTest(it, model,
        "250 million dutch guilders of 7 3 / 4 % bonds due nov . 15 , 1999 , priced at 101 1 / 4 to yield 7 . 57 % at issue price and 7 . 86 % less full fees , via amro bank .",
        "250000000 Netherlands guilder");

    basicTest(it, model,
        "in addition , the bank has an option to buy a 30 . 84 % stake in bip from societe generale after jan . 1 , 1990 at 1,015 francs a share .",
        "1015 Franc");

    basicTest(it, model,
        "its shares slid in late dealings to close one penny",
        "1 Penny");

    basicTest(it, model,
        "per share lower at 197 pence.",
        "197 Pence");

    basicTest(it, model,
        "its quarterly operating profit improved to 361 million pounds ",
        "361000000 Pound");

    basicTest(it, model,
        "last year , the gross output value of township enterprises of the whole city broke through 100 billion yuan for the first time , ranking first in the entire province .",
        "100000000000 Chinese yuan");

    basicTest(it, model,
        "rangers got to keep an estimated £ 50 million saved by baxendale - walker ' s advice .",
        "50000000 Pound");

    basicTest(it, model,
        "in turn , francis leung pak - to has agreed to sell an 8 % stake in pccw to telefónica for 323 million euros .",
        "323000000 Euro");

    basicTest(it, model,
        "uefa charged ferguson for bringing the game into disrepute with his comments , and on 1 may that year he was fined 10,000 swiss francs  .",
        "10000 Swiss franc");

    basicTest(it, model,
        "the ipl signed up kingfisher airlines as the official umpire partner for the series in a ( approximately £ 15 million ) deal .",
        "15000000 Pound");

    basicTest(it, model,
        "the revenue of adelaide ' s electronics industry has grown at about 15 % per annum since 1990 , and in 2011 exceeds a $ 4 billion .",
        "4000000000 Dollar");

    basicTest(it, model,
        "abel and associates bid $ 4 million for doing the film ' s effects and paramount accepted .",
        "4000000 Dollar");

    basicTest(it, model,
        "uefa charged ferguson for bringing the game into disrepute with his comments , and on 1 may that year he was fined 10,000 swiss francs  .",
        "10000 Swiss franc");

    basicTest(it, model,
        "malone sued 20th century - fox for $ 1.6 million for breach of contract ;",
        "1600000 Dollar");

    basicTest(it, model,
        "in 2003 , bayern munich loaned € 2 million to dortmund for a couple of months to pay their payroll .",
        "2000000 Euro");

    basicTest(it, model,
        "lockheed martin and the united states government intensively lobbied for india ' s us $ 10 billion contract for 126 fighter jets .",
        "10000000000 United States dollar");

    basicTest(it, model,
        "according to research firm npd , the average selling price of all windows portable pcs has fallen from $ 659 in october 2008 to",
        "659 Dollar");

    basicTest(it, model,
        "one . tel floated on the australian stock exchange at $ 2 per share in november 1997 .",
        "2 Dollar");

    basicTest(it, model,
        "the east stand ( worcester avenue ) stand was finished in 1934 and this increased capacity to around 80 , 000 spectators but cost £ 60,000 .",
        "60000 Pound");

    basicTest(it, model,
        "his fulham teammate johnny haynes became the first £ 100 player .",
        "100 Pound");

    basicTest(it, model,
        "for the nine months , amr ' s net rose 15 % to $ 415.9 million",
        "415900000 Dollar");

    basicTest(it, model,
        "the airline ' s share price already is far below the 210 pence  level seen after the company announced the rights issue in late september .",
        "210 Pence");

    basicTest(it, model,
        "rolling stone noted , ` ` harpercollins acquired the book project for $ 3 million in 2008 .",
        "3000000 Dollar");

    basicTest(it, model,
        "their conclusion was a terse pronouncement that $ 48 \" is not adequate . \"",
        "48 Dollar");

    basicTest(it, model,
        "2013 , edition of forbes magazine features keith on the cover with the caption ` ` country music ' s $ 500 million man ' ' .",
        "500000000 Dollar");

    basicTest(it, model,
        "harry ferguson sued us ford for illegal use of his patents asking for compensation of £ 90 million , settled out of court in 1952 .",
        "90000000 Pound");

    basicTest(it, model,
        "aerosmith signed with columbia in mid - 1972 for a reported $ 125,000 and issued their debut album , aerosmith .",
        "125000 Dollar");

    basicTest(it, model,
        "it was one of coke ' s largest acquisitions since it bought odwalla inc . for $ 186 million in 2001 .",
        "186000000 Dollar");

    basicTest(it, model,
        "subsequently , apple and creative reached a settlement , with apple paying $ 100 million to creative , and creative joining the ` ` made for ipod ' ' accessory program .",
        "100000000 Dollar");

    basicTest(it, model,
        "in turn , francis leung pak - to has agreed to sell an 8 % stake in pccw to telefónica for 323 million euros .",
        "323000000 Euro");

    basicTest(it, model,
        "malone sued 20th century - fox for $ 1.6 million for breach of contract ;",
        "1600000 Dollar");

    basicTest(it, model,
        "in 2003 , bayern munich loaned € 2 million to dortmund for a couple of months to pay their payroll .",
        "2000000 Euro");

    basicTest(it, model,
        "lockheed martin and the united states government intensively lobbied for india ' s us $ 10 billion contract for 126 fighter jets .",
        "10000000000 United States dollar");

    multiTest(it, model,
        "the hart - scott filing is then reviewed and any antitrust concerns usually met . typically , hart - scott is used now to give managers of target firms early news of a bid and a chance to use regulatory review as a delaying tactic . the $ 20,000 tax would be a small cost in a multibillion - dollar deal , but a serious drag on thousands of small , friendly deals .",
        ["20000 Dollar", "Dollar"]);

    multiTest(it, model,
        "dollar : 143.80 yen , up 0 . 95 ; 1 . 8500 marks , up 0 . 0085 .",
        ["143.8 Japanese yen", "Dollar"]);
});

describe('Temperature .', it => {
    let model = NumberWithUnitRecognizer.instance.getTemperatureModel(Culture.English, false);

    basicTest(it, model, "the temperature outside is 40 deg celsius", "40 C");

    basicTest(it, model, "its 90 fahrenheit in texas", "90 F");

    basicTest(it, model, "-5 degree fahrenheit", "-5 F");

    basicTest(it, model, "6 deg c", "6 C");

    basicTest(it, model, "98.6 degrees f is normal temperature", "98.6 F");

    basicTest(it, model, "set the temperature to 30 degrees celsius", "30 C");

    basicTest(it, model, "normal temperature is 98.6 degrees fahrenheit", "98.6 F");

    basicTest(it, model, "100 degrees f", "100 F");

    basicTest(it, model, "20 degrees c", "20 C");

    basicTest(it, model, "100.2 degrees farenheit is low", "100.2 F");

    basicTest(it, model, "10.5 celcius", "10.5 C");

    basicTest(it, model, "20 degrees celsius", "20 C");

    basicTest(it, model, "20.3 celsius", "20.3 C");

    basicTest(it, model, "34.5 celcius", "34.5 C");

    basicTest(it, model, "the temperature outside is 98 degrees", "98 Degree");

    basicTest(it, model, "set the thermostat to 85°", "85 Degree");

    basicTest(it, model, "raise the temperature by 5 degrees", "5 Degree");

    basicTest(it, model, "set the temperature to 70 degrees f", "70 F");

    basicTest(it, model, "raise the temperature by 20 degrees", "20 Degree");

    basicTest(it, model, "set the temperature to 100 degrees", "100 Degree");

    basicTest(it, model, "keep the temperature at 75 degrees f", "75 F");

    basicTest(it, model, "let the temperature be at 40 celsius", "40 C");

    basicTest(it, model, "let the temperature be at 50 deg. ", "50 Degree");

    multiTest(it, model, "convert 10 celsius to fahrenheit", ["10 C", "F"]);

    multiTest(it, model, "34.9 centigrate to farenheit", ["34.9 C", "F"]);

    multiTest(it, model, "convert 200 celsius celsius into fahrenheit", ["200 C", "C", "F"]);

    multiTest(it, model, "fahrenheit to celsius 101 fahrenheit is how much celsius", ["101 F", "F", "C", "C"]);

    multiTest(it, model, "50 degrees celsius celsius to fahrenheit", ["50 C", "C", "F"]);

    multiTest(it, model, "could you convert 51 fahrenheit to degrees celsius", ["51 F", "C"]);

    multiTest(it, model, "convert 106 degree fahrenheit to degrees celsius", ["106 F", "C"]);

    multiTest(it, model, "convert 45 degrees fahrenheit to celsius", ["45 F", "C"]);

    multiTest(it, model, "how to convert - 20 degrees fahrenheit to celsius", ["-20 F", "C"]);
});

describe('Dimension .', it => {
    var model = NumberWithUnitRecognizer.instance.getDimensionModel(Culture.English, false);
    basicTest(it, model,
        "75ml",
        "75 Milliliter");

    basicTest(it, model,
        "its greatest drawback may be its 3-inch thickness , big enough for one consultant to describe it as ` ` clunky . ' '",
        "3 Inch");

    basicTest(it, model,
        "a twister roared through an area about ten miles long there , killing at least fourteen people and turning dozens of homes into rubble .",
        "10 Mile");

    basicTest(it, model,
        "it takes more than 10 1/2 miles of cable and wire to hook it all up , and 23 computers .",
        "10.5 Mile");

    basicTest(it, model,
        "the six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours .",
        "6 Mile");

    basicTest(it, model,
        "industrywide , oil production in this country fell by 500,000 barrels a day to barrels in the first eight months of this year .",
        "500000 Barrel");

    basicTest(it, model,
        "it ' s what 1 ) explains why we are like , well , ourselves rather than bo jackson ; 2 ) cautions that it ' s possible to drown in a lake that averages two feet deep ; and 3 ) predicts that 10 , 000 monkeys placed before 10 , 000 pianos would produce 1 , 118 publishable rock ' n ' roll tunes .",
        "2 Foot");

    basicTest(it, model,
        "on may 19 , the fda began detaining chinese mushrooms in 68-ounce cans after more than 100 people in mississippi , new york and pennsylvania became ill from eating tainted mushrooms .",
        "68 Ounce");

    basicTest(it, model,
        "mr . hulings gloats that he sold all his stocks a week before the market plummeted 190 points on oct . 13 , and he is using the money to help buy a 45-acre horse farm .",
        "45 Acre");

    basicTest(it, model,
        "then , to make these gardenettes quite literally rooms , ms . bartlett had thrown up windowless walls ( brick , lattice , hedge ) eight to 10 feet tall , casting her interiors into day - long stygian shade .",
        "10 Foot");

    basicTest(it, model,
        "` ` management does n ' t want surprises , ' ' notes jack zaves , who , as fuel - services director at american airlines , buys some 2.4 billion gallons of jet fuel a year .",
        "2400000000 Gallon");

    basicTest(it, model,
        "a 10-gallon water cooler had toppled onto the floor , soaking the red carpeting .",
        "10 Gallon");

    basicTest(it, model,
        "nearby , six dolphins will frolic in a 1.5 million gallon saltwater aquarium .",
        "1500000 Gallon");

    basicTest(it, model,
        "and this baby is over two pounds .",
        "2 Pound");

    basicTest(it, model,
        "` ` i do n ' t trust people who do n ' t eat , ' ' said ms . volokh , though she herself stopped eating lunch a few years ago to drop 25 pounds .",
        "25 Pound");

    basicTest(it, model,
        "shell , a subsidiary of royal dutch / shell group , will be allowed to export 0.9 trillion cubic feet , and gulf , a unit of olympia & york developments ltd . will be allowed to export",
        "900000000000 Cubic foot");

    basicTest(it, model,
        "highlights of the bills , as currently framed , are : - - a restriction on the amount of real estate one family can own , to 660 square meters in the nation ' s six largest cities , but more in smaller cities and rural areas .",
        "660 Square meter");

    basicTest(it, model,
        "tigrean armies are now 200 miles north of addis ababa , threatening the town of dese , which would cut off mr . mengistu ' s capital from the port of assab , through which all fuel and other supplies reach addis ababa .",
        "200 Mile");

    basicTest(it, model,
        "he said that one of the computers took a three-foot trip sliding across the floor .",
        "3 Foot");

    basicTest(it, model,
        "the core of its holdings is 190,000 square meters of incredibly expensive property in the marunouchi district , the business and financial center of tokyo , often jokingly called ` ` mitsubishi village . ' '",
        "190000 Square meter");

    basicTest(it, model,
        "the satellite , built by hughes for the international telecommunications satellite organization , is part of a $ 700 million contract awarded to hughes in 1982 to develop five of the three-ton satellites .",
        "3 Ton");


    basicTest(it, model,
        "in a 1996 report on biological weapons , the center for strategic and international studies , a public policy research institution in washington , warned that it was easy for would - be terrorists to assemble biological weapons _ using commercial equipment with a capacity of 130 gallons .",
        "130 Gallon");

    basicTest(it, model,
        "the trade group ' s compilation of commerce department data showed that august imports , the second largest monthly total of the year , were up 5 % from july ' s 1,458,000 tons but below last year ' s high of in june 1988 .",
        "1458000 Ton");

    basicTest(it, model,
        "at no . 1 , singh hit a 9 - iron approach shot to within six feet of the cup .",
        "6 Foot");

    basicTest(it, model,
        "so when next year ' s psyllium crop is harvested in march , it may be smaller than the 16,000 metric tons of the past few years - - right at the crest of the psyllium boom .",
        "16000 Metric ton");

    basicTest(it, model,
        "the 486 is the descendant of a long series of intel chips that began dominating the market ever since ibm picked the 16-bit 8088 chip for its first personal computer .",
        "16 Bit");

    basicTest(it, model,
        "the ` ` jiotto caspita ' ' can run at over 188 miles an hour , a company spokesman said .",
        "188 Mile per hour");

    basicTest(it, model,
        "the navy has set up a helicopter landing zone just a 100 meters from a mobile operating room , just on the outskirts of baghdad .",
        "100 Meter");

    basicTest(it, model,
        "caltrans plans to add a second deck for buses and car pools above the median of a 2.5-mile stretch of the harbor freeway just south of los angeles , near the memorial coliseum .",
        "2.5 Mile");

    basicTest(it, model,
        "on my four-mile drive to farm headquarters each morning , i drive by another four empty houses .",
        "4 Mile");

    basicTest(it, model,
        "we are insulted , \" said langa from the greek catholic headquarters , some 325 kilometer northwest of bucharest .",
        "325 Kilometer");

    basicTest(it, model,
        "rotich is a tiny ( 5 feet",
        "5 Foot");

    basicTest(it, model,
        "4 inches) 28 - year - old who did not start running seriously until three years ago and had not competed indoors until this month .",
        "4 Inch");

    basicTest(it, model,
        "raceway park ( minnesota ) in shakopee is a 1/4 mile paved oval .",
        "0.25 Mile");

    basicTest(it, model,
        "castlecrag mountain is located south of moat lake , 1.6 km west of mount frink along the same ridge line .",
        "1.6 Kilometer");

    basicTest(it, model,
        "the javadi hills are located about 17 km from ambur .",
        "17 Kilometer");

    basicTest(it, model,
        "after circling lake michigan near the exposition for two hours , commander hugo eckener landed the 776-foot airship at the nearby curtiss - wright airport in glenview .",
        "776 Foot");

    basicTest(it, model,
        "the interchange with highway 35 and highway 115 to lindsay and peterborough ( exit 436 ) lies 500 metres east of bennett road .",
        "500 Meter");

    basicTest(it, model,
        "sterling armaments of dagenham , essex produced a conversion kit comprising a new 7.62mm barrel , magazine , extractor and ejector for commercial sale .",
        "7.62 Millimeter");

    basicTest(it, model,
        "the project costs $ 46 . 8 million , and is intended to boost the company ' s production capacity by 25 % to 34,500 metric tons of copper cathode a year .",
        "34500 Metric ton");

    basicTest(it, model,
        "canadian steel - ingot production totaled 291,890 metric tons in the week ended oct . 7 , up 14 . 8 % from the preceding week ' s total of , statistics canada , a federal agency , said .",
        "291890 Metric ton");

    basicTest(it, model,
        "florida panthers live in home ranges between 190 km2 .",
        "190 Square kilometer");

    multiTest(it, model,
        "a metric ton is equal to 2,204.62 pounds .",
        ["2204.62 Pound", "Metric ton"]);
});

describe('Age .', it => {
    var model = NumberWithUnitRecognizer.instance.getAgeModel(Culture.English, false);

    basicTest(it, model,
        "When she was five years old, she learned to ride a bike.",
        "5 Year");

    basicTest(it, model,
        "This saga is ten years old.",
        "10 Year");

    basicTest(it, model,
        "I'm only 29 years old!",
        "29 Year");

    basicTest(it, model,
        "Now, after ninety five years of age, perspectives change.",
        "95 Year");

    basicTest(it, model,
        "The Great Wall of China is more than 500 years old and extends for more than 5,000 miles.",
        "500 Year");

    basicTest(it, model,
        "She's 60 years old; she was born in May 8, 1945.",
        "60 Year");

    basicTest(it, model,
        "25% of cases are not diagnosed until around 3 years of age.",
        "3 Year");

    basicTest(it, model,
        "When will there be pressure to fulfil a promise that is one year old?",
        "1 Year");

    basicTest(it, model,
        "It happened when the baby was only ten months old.",
        "10 Month");

    basicTest(it, model,
        "The committee proposal is 8 months old.",
        "8 Month");

    basicTest(it, model,
        "Aproximately 50% of cases are diagnosed at around eighteen months of age.",
        "18 Month");

    basicTest(it, model,
        "It is possible, but in 2006 95% of them were younger than three months old.",
        "3 Month");

    basicTest(it, model,
        "If we go ahead in December, it will be three weeks old.",
        "3 Week");

    basicTest(it, model,
        "At 6 weeks of age, one can already celebrate Christmas.",
        "6 Week");

    basicTest(it, model,
        "A 90 day old utilities bill is quite late.",
        "90 Day");
});

function basicTest(it, model, source, value) {
    it(source, t => {
        t.not(model, null);
        let result = model.parse(source);
        t.is(result.length, 1);
        t.is(result[0].resolution["value"] + " " + result[0].resolution["unit"], value);
    });
}

function multiTest(it, model, source, values) {
    it(source, t => {
        t.not(model, null);
        let result = model.parse(source);
        t.is(result.length, values.length);
        t.deepEqual(result.map(r => (r.resolution["value"] + " " + r.resolution["unit"]).trim()), values);
    });
}