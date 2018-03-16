
def parse_string(timex, obj):

    if timex == 'PRESENT_REF':
        obj.Now = True

    elif timex.startswith('P'):
        extract_duration(timex, obj)

    elif timex.startswith('(') && timex.endswith(')'):
        extract_start_end_range(timex, obj)

    else:
        extract_date_time(timex, obj)

def extract_duration(s, obj):
    pass

def extract_start_end_range(timex, obj):
    pass

def extract_date_time(s, timex):
    pass
