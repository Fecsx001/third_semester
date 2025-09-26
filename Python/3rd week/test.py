from typing import Callable


def decorator(func):
    def wrapper(*args, **kwargs):
        print("Before")
        result = func(*args, **kwargs)
        print("After")
        return result

    return wrapper


@decorator
def setup_mult(x: int) -> Callable:
    def mult(y: int):
        return x * y

    return mult


double: Callable = setup_mult(2)
triple: Callable = setup_mult(3)

print(double(5))


from time import time, sleep


def super_add(a: int, b: int) -> int:
    now = time()
    sleep(a)
    sleep(b)
    return time() - now
