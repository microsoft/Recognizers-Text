class EnglishConstants:
    DAYS: [str] = [
        'Monday',
        'Tuesday',
        'Wednesday',
        'Thursday',
        'Friday',
        'Saturday',
        'Sunday'
    ]

    MONTHS: [str] = [
        'January',
        'Februrary',
        'March',
        'April',
        'May',
        'June',
        'July',
        'August',
        'September',
        'October',
        'November',
        'December'
    ]

    DATE_ABBREVIATION: {int, str} = {
        0: 'th',
        1: 'st',
        2: 'nd',
        3: 'rd',
        4: 'th',
        5: 'th',
        6: 'th',
        7: 'th',
        8: 'th',
        9: 'th'
    }

    HOURS: [str] = [
        'midnight', '1AM', '2AM', '3AM', '4AM', '5AM', '6AM', '7AM', '8AM', '9AM', '10AM', '11AM',
        'midday', '1PM', '2PM', '3PM', '4PM', '5PM', '6PM', '7PM', '8PM', '9PM', '10PM', '11PM'
    ]

    SEASONS: {str, str} = {
        'SP': 'spring',
        'SU': 'summer',
        'FA': 'fall',
        'WI': 'winter'
    }

    WEEKS: [str] = ['first', 'second', 'third', 'forth']

    DAY_PARTS: {str, str} = {
        'DT': 'daytime',
        'NI': 'night',
        'MO': 'morning',
        'AF': 'afternoon',
        'EV': 'evening'
    }
