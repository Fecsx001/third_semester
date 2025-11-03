class A():
    counter = 0

    def __init__(self):
        self.counter = 42  # Private variable

    def modify(self, cls):
        cls.counter += 1
        self.counter += 1
        print(self.counter + cls.counter)


a = A()
a.modify(a)