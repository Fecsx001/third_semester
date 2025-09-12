import json

# Create a list
a = []
# Add my items to it

a = [i for i in range(0, 70, 2)]

print(a[0])
print(a[-1])

# Get input and returning the square of it
b = int(input("Enter a number: "))
print(b**2)

# Creating a complex number
c = 3 + 5j
print(c)
print(type(c))

# printing the real and imaginary part
print(c.real)
print(c.imag)


def word_in_it(word: str, dictionary: dict) -> None:
    if word in dictionary:
        dictionary[word] += 1
    else:
        dictionary[word] = 1


# make it usable with user inputs
word = input("Enter a word: ")
dictionary_input = input("Enter a dictionary (in JSON format): ")

try:
    # Parse the input into a dictionary
    dictionary = json.loads(dictionary_input)
    print(type(dictionary))

    if word in dictionary:
        print("Word found")
        dictionary[word] += 1
    else:
        dictionary[word] = 1

    print(dictionary)
except json.JSONDecodeError:
    print("Invalid dictionary format. Please enter a valid JSON string.")
