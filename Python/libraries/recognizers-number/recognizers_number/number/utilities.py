#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from decimal import localcontext


def precision(*args, **kwargs):
    def decorator(f):
        def inner_decorator(*a, **kwa):
            with localcontext() as ctx:
                ctx.prec = kwargs['prec']
                return f(*a, **kwa)

        return inner_decorator

    return decorator
