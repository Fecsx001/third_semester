from time import time
from functools import wraps

# 15. Feladat
# Írj egy dekorátort, amely megszámolja, hogy egy függvény hányszor lett meghívva


def count_calls(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        wrapper.calls += 1
        print(f"{func.__name__} {wrapper.calls} alkalommal lett meghívva")
        return func(*args, **kwargs)

    wrapper.calls = 0
    return wrapper


# 16. Feladat
# Írj egy dekorátort, amely méri, hogy egy függvény futása mennyi időt vett igénybe


def timer(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        start = time()
        result = func(*args, **kwargs)
        run = time() - start
        print(run)
        return result

    return wrapper


# 17. Feladat
# Írj egy dekorátort, amely egy függvény hívását valamilyen vizuális keretbe helyezi


def visualize(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        print(f"{func.__name__} started running")
        result = func(*args, **kwargs)
        print(f"{func.__name__} finished running")
        return result

    return wrapper


# 18. Feladat
# Írj egy dekorátort, amely visszaadja, hogy egy függvény milyen módon és milyen paraméterekkel lett meghívva


def args(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        print(f"{func.__name__} was called with args: {args} and kwargs: {kwargs}")
        result = func(*args, **kwargs)
        print(f"{func.__name__} finished running")
        return result

    return wrapper


# ----------Próbáld ki a 18. feladatban implementált dekorátort--------------------


# Fordíts meg rekurzívan egy stringet
def reverse_string(s):
    if len(s) == 1:
        return s
    else:
        return f"{reverse_string(s[1:])}{s[0]}"


# Számold ki rekurzívan két tömb metszetét
def intersection(lst1, lst2):
    if not lst1:
        return []
    head, *tail = lst1
    if head in lst2:
        return [head] + intersection(tail, lst2)
    else:
        return intersection(tail, lst2)


if __name__ == "__main__":
    print(intersection([1, 2, 3, 4, 5], [3, 4, 5, 6, 7]))

# Irj egy függvényt ami megtalálja az alábbi fa szerű listában a maximum element
l_1 = [1, [2, 3, [4, 5]], [6, 7], 8, [9, 10, [11, 12]]]
l_2 = [16, [2, 3, [14, 5]], [6, 17], 8, [9, 10, [11, 12]]]


def find_max(lst):
    current_max = float("-inf")
    for elem in lst:
        if isinstance(elem, list):
            current_max = max(current_max, find_max(elem))
        else:
            current_max = max(current_max, elem)
    return current_max


print(find_max(l_1))
print(find_max(l_2))
