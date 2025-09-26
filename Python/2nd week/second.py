# Írj egy függvényt ami egy számot vár paraméterül és visszaadja a szám négyzetét!
def square_number(a: int | float) -> int | float:
    return a**2


# Írj egy függvényt ami két számot vár paraméterül és kiírja a nagyobbat!
def compare_numbers(a, b):
    print(a if a > b else b)


# Írj egy függvényt ami egy számot vár paraméterül és visszaadja a szám abszolút értékét!(abs() kulcszó)
def absolute_value(a):
    return abs(a)


# Írj egy függvényt ami egy stringet vár paraméterül és megadja a string hosszát! (len() kulcszó)
def string_length(s):
    return len(s)


# Írj egy függvényt ami egy stringet vár paraméterül és kiírja a string első karakterét!:
def first_char(s):
    print(s[0])


# Írj egy függvényt ami egy stringet vár paraméterül majd kiírja az utolsó karakterét!:
def last_char(s):
    print(s[-1])


# Írj egy függvényt ami egy stringet vár paraméterül majd kiírja a középső karakterét!:
import math


# math.floor
def middle_char(s):
    print(s[math.floor(len(s))])


# Írj egy függvényt ami egy három elemű tömböt vár paraméterül és kiírja a tömb első és utolsó elemét!
def unpack_list_first_last(elements):
    if len(elements) != 3:
        raise ValueError("Not 3 elements")
    print(f"First: {elements[0]}, last: {elements[-1]}")


# Írj egy függvényt, ami egy háromszög két oldalát várja és visszaadja a területét! (Hsz képlet: sqrt(s * (s - a) * (s - b) * (s - c))
# ahol s = a + b + c / 2)
def triangle_area(a, b, c):
    s = a + b + c / 2
    return math.sqrt(s * (s - a) * (s - b) * (s - c))


# Írj egy függvényt, ami egy számot vár paraméterül és visszaadja a szám hárommal vett maradékát! (%)
def modulo_3(a):
    return a % 3


# Írj egy függvényt, ami egy számot vár paraméterül, és visszaadja, hogy a szám páros vagy páratlan!
def is_even(a):
    return "even" if a % 2 == 0 else "odd"


# Írj egy függvényt, ami egy listát vár paraméterül, és visszaadja a lista legnagyobb elemét! (Használd a max() kulcsszót)
def max_in_list(lst):
    return max(lst)


# Írj egy függvényt, ami két stringet vár paraméterül és visszaadja, hogy az első string tartalmazza-e a másodikat! (Használd az "in" operátort)
def contains_string(s1, s2):
    return s2 in s1


# Írj egy függvényt, ami egy listát vár paraméterül és visszaadja a lista elemeinek összegét! (Használd a sum() kulcsszót)
def sum_of_list(lst):
    return sum(lst)


# Írj egy függvényt, ami egy számot vár paraméterül és visszaadja, hogy a szám pozitív, negatív vagy nulla!
def check_number(a):
    return "positive" if a > 0 else ("negative" if a < 0 else "null")


# Írj egy függvényt, ami egy stringet vár paraméterül és visszaadja a stringet nagybetűkkel! (Használd az upper() függvényt)
def string_upper(s: str) -> str:
    return s.upper()


# Írj egy függvényt, ami egy stringet vár paraméterül és visszaadja a stringet kisbetűkkel! (Használd a lower() függvényt)
def string_lower(s: str) -> str:
    return s.lower()


# Írj egy függvényt, ami egy stringet vár paraméterül és visszaadja a stringet olyan módon, hogy az első betűje nagy a többi kicsi! (Használd a title() függvényt)
def string_title(s: str) -> str:
    return s.title()


# ---------------------------Hard exercises---------------------------

# Írj egy függvényt ami egy listát vár paraméterül és rekurzívan visszaadja a lista páros maximumát
# [1,2,3] -> 2, [1,3,5] -> None, [7,5,3,2,4] -> 4

# Implementáld a tic-tac-toe játékot két játékos részére, command line printekkel
# PL:
# x O X
# O X O
# O X X

# A második játékost cseréld le egy "AI"-ra, aki véletlenszerűen választ egy üres helyet
# Hasznéld a random könyvtárat
# import random

# A második játékos ha tud nyerni akkor nyerjen, máskülönben maradjon a véletlenszerű választásnál

# Implementálj egy függvényt foo(n), ami rekurzívan kiszámolja mind az n faktoriálisát, mind az n!-ik fibonacci számot.
