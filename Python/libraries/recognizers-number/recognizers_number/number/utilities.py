from decimal import localcontext


def precision(*args, **kwargs):
    def decorator(f):
        def inner_decorator(*a, **kwa):
            with localcontext() as ctx:
                ctx.prec = kwargs['prec']
                return f(*a, **kwa)

        return inner_decorator

    return decorator
