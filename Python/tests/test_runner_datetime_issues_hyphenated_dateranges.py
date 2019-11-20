import recognizers_suite as Recognizers
from recognizers_suite import Culture, ModelResult
culture = Culture.English

# case 1: good
# input = "10/1-11/5"
# text: 10/1-11/5
# type_name: datetimeV2.daterange
# resolution: {'values': [{'timex': '(XXXX-10-01,XXXX-11-05,P35D)', 'type': 'daterange', 'start': '2019-10-01', 'end': '2019-11-05'}]}

# case 2: bad
input = "Nov-Dec 2015"
# text: nov
# type_name: datetimeV2.daterange
# resolution: {'values': [{'timex': 'XXXX-11', 'type': 'daterange', 'start': '2018-11-01', 'end': '2018-12-01'}, {'timex': 'XXXX-11', 'type': 'daterange', 'start': '2019-11-01', 'end': '2019-12-01'}]}
# text: dec 2015
# type_name: datetimeV2.daterange
# resolution: {'values': [{'timex': '2015-12', 'type': 'daterange', 'start': '2015-12-01', 'end': '2016-01-01'}]}

# case 3: good ok
#input = "Nov 2015 - Dec 2015"
# text: nov 2015
# type_name: datetimeV2.daterange
# resolution: {'values': [{'timex': '2015-11', 'type': 'daterange', 'start': '2015-11-01', 'end': '2015-12-01'}]}
# text: dec 2015
# type_name: datetimeV2.daterange
# resolution: {'values': [{'timex': '2015-12', 'type': 'daterange', 'start': '2015-12-01', 'end': '2016-01-01'}]}

# case 4: good
#input = "My vacation is from 10/1/2018 - 10/7/2018"
# text: from 10/1/2018 - 10/7/2018
# type_name: datetimeV2.daterange
# resolution: {'values': [{'timex': '(2018-10-01,2018-10-07,P6D)', 'type': 'daterange', 'start': '2018-10-01', 'end': '2018-10-07'}]}

# case 5: bad
#input = "My vacation is from 10-1-2018-10-7-2018"
results = Recognizers.recognize_datetime(input, culture)
for result in results:
    print("text: {}".format(result.text))
    print("type_name: {}".format(result.type_name))
    print("resolution: {}".format(result.resolution))
