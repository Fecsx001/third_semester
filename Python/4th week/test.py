from typing import Any, Iterable

# Exceptions

# 1. Irj egy függvényt ami felhasználói bemenetet kér, majd számmá alakítja és és elosztja vele az 1-et. (1 / input)
# Kezeld le az összes lehetséges hibát, és írj megfelelő hibaüzenetet.


def safe_int(x: Any) -> int:
    try:
        if not isinstance(Iterable, x):
            raise TypeError(f"Can't convert type {type(x)} to int")
        if not all(str(x).isdigit()):
            raise TypeError(f"The variable contains not only digits")
        x = float(x)
    except TypeError as e:
        raise TypeError(f"Not able to convert to int due to: {str(e)}")


# 2. Irj egy végtelen ciklust, ami addig iterál amíg a felhasználó ctrl+c-vel meg nem szakítja.
# Ebben az esetben kezeld le a KeyboardInterrupt kivételt és írj egy üzenetet, hogy a program leállt.


def infinite():
    try:
        while True:
            pass
    except KeyboardInterrupt:
        print("A programot leállítottad Ctrl+C-vel.")



# ----------------------------------------------------  Generators + Yield + Lambdas ----------------------------------------------------
# 1. Feladat: Lambda két szám szorzására
multiply = lambda x, y: y * x
print(multiply(2, 3))  # Kimenet: 6

# 2. Feladat: Lambda két szám maximumának megtalálására
maximum = lambda x, y: x if x > y else y  # max(x, y)
print(maximum(2, 3))  # Kimenet: 3

# 3. Feladat: Lambda egy szám páros voltának ellenőrzésére
is_even = lambda x: x % 2 == 0
print(is_even(4))  # Kimenet: True

# 4. Feladat: Lambda egy string megfordítására
reverse_string = lambda x: "".join(list(reversed(x)))
print(reverse_string("hello"))  # Kimenet: "olleh"

# 5. Feladat: Lambda egy szám négyzetének kiszámítására
square = lambda x: x**2
print(square(4))  # Kimenet: 16

# 6. Feladat: Lambda páros számok szűrésére egy listából
filter_even = lambda x: list(filter(lambda y: y % 2 == 0, x))
print(filter_even([1, 2, 3, 4, 5, 6]))  # Kimenet: [2, 4, 6]

# 7. Feladat: Lambda stringek listájának nagybetűssé alakítására
to_uppercase = lambda x : [y.upper() for y in x]
print(to_uppercase(["hello", "world"]))  # Kimenet: ["HELLO", "WORLD"]

# 8. Feladat: Lambda tuple-ök listájának rendezésére a második elem alapján
sort_by_second = lambda x : [tuple(reversed(y)) for y in x]
print(sort_by_second([(1, 3), (2, 2), (3, 1)]))  # Kimenet: [(3, 1), (2, 2), (1, 3)]

# 9. Feladat: Lambda stringek hosszának megtalálására egy listában
lengths = lambda x : [len(y) for y in x]
print(lengths(["hello", "world"]))  # Kimenet: [5, 5]

# 10. Feladat: Lambda egy konstans hozzáadására minden elemhez egy listában
add_constant = lambda x, y : [z+y for z in x]
print(add_constant([1, 2, 3], 5))  # Kimenet: [6, 7, 8]

# 11. Feladat: Lambda két lista metszetének megtalálására
intersection = lambda x, y : [z for z in x if z in y]
print(intersection([1, 2, 3], [2, 3, 4]))  # Kimenet: [2, 3]

# 12. Feladat: Lambda annak ellenőrzésére, hogy egy string csak számjegyeket tartalmaz-e
is_digit = lambda x : x.isdigit()
print(is_digit("123"))  # Kimenet: True

# 13. Feladat: Lambda duplikátumok eltávolítására egy listából
remove_duplicates = None  # TODO
# print(remove_duplicates([1, 2, 2, 3, 4, 4, 5]))  # Kimenet: [1, 2, 3, 4, 5]

# 14. Feladat: Lambda egy szám faktoriálisának kiszámítására
factorial = None  # TODO
# print(factorial(5))  # Kimenet: 120

# 15. Feladat: Lambda annak ellenőrzésére, hogy egy string palindróm-e
is_palindrome = None  # TODO
# print(is_palindrome("racecar"))  # Kimenet: True
