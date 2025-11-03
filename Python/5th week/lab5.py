import hashlib
import os



# készíts egy osztályt ami baki felhasználókat ír le
# valamint egyet ami banki felhasználókat tárol
class BankAccount:
    def __init__(self, name, email, phone, account_number, password):
        self.__name = name
        self.__email = email
        self.__phone = phone
        self.__account_number = account_number
        self.__password = hashlib.sha256(password)

        self.__salt = os.urandom(16)
        self.__password_hash = self.__hash_password(password)

    def __hash_password(self, password: str) -> str:
        return hashlib.sha256(self.__salt + password.encode()).hexdigest()

    def get_name(self):
        return self.__name
    def get_email(self):
        return self.__email
    def get_phone(self):
        return self.__phone
    def get_account_number(self):
        return self.__account_number
    def get_password(self):
        return self.__password

    def set_name(self, name):
        self.__name = name
    def set_email(self, email):
        self.__email = email
    def set_phone(self, phone):
        self.__phone = phone
    def set_account_number(self, account_number):
        self.__account_number = account_number
    def set_password(self, password):
        self.__password = hashlib.sha256(password)

    def verify_password(self, password: str) -> bool:
        """Check if given password matches stored hash."""
        return self.__password_hash == hashlib.sha256(self.__salt + password.encode()).hexdigest()

    def __str__(self):
        return f"Bank account object with values: name: {self.__name}, email: {self.__email}, phone: {self.__phone}"

    def __eq__(self, other):
        return self.__account_number == other.get_account_number() and self.__email == other.get_email() and self.__phone == other.get_phone()

    def __lt__(self, other):
        return self.get_account_number() < other.get_account_number()

    def __hash__(self):
        return hash(self.__account_number)

    def __del__(self):
        print(f"Account: {self.get_name()}, email: {self.get_email()}, phone: {self.get_phone()} deleted")
# Feladat:
# legyenek privát módon tárolva a következő változók:
# név email cím telefonszám bankszámlaszám egyenleg jelszó hash kódja
# ---> opcionálisan legyenek validálva példányosításkor
# mindegyikhez legyen egy getter és setter
# definiáld felül a __str__, __eq__, __lt__, __hash__ és del metódusokat

class Users:
    def __init__(self):
        self.__users: dict[BankAccount, tuple[str, str]] = {}
        self.__loans: dict[BankAccount, float] = {}

    def add_user(self, account: BankAccount, username: str, password: str):
        password_hash = hashlib.sha256(password.encode()).hexdigest()
        self.__users[account] = (username, password_hash)

    def verify_user(self, account: BankAccount, password: str) -> bool:
        """Verify password for login/transactions."""
        if account not in self.__users:
            return False
        _, stored_hash = self.__users[account]
        return stored_hash == hashlib.sha256(password.encode()).hexdigest()

    def transaction(self, sender: BankAccount, receiver: BankAccount, amount: float, password: str):
        """Money transfer between accounts."""
        if not self.verify_user(sender, password):
            print("❌ Incorrect password.")
            return
        if sender.get_balance() < amount:
            print("❌ Insufficient funds.")
            return

        sender.set_balance(sender.get_balance() - amount)
        receiver.set_balance(receiver.get_balance() + amount)
        print(f"✅ Transferred {amount} from {sender.get_name()} to {receiver.get_name()}")

    def withdraw(self, account: BankAccount, amount: float, password: str):
        """Withdraw money from an account."""
        if not self.verify_user(account, password):
            print("❌ Incorrect password.")
            return
        if account.get_balance() < amount:
            print("❌ Insufficient funds.")
            return

        account.set_balance(account.get_balance() - amount)
        print(f"✅ {amount} withdrawn from {account.get_name()}")

    def close_account(self, account: BankAccount, password: str):
        """Delete user account."""
        if not self.verify_user(account, password):
            print("❌ Incorrect password.")
            return
        del self.__users[account]
        print(f"🗑️ Account for {account.get_name()} closed.")

    # --- Loan management ---
    def request_loan(self, account: BankAccount, amount: float, interest_rate=0.05):
        """Grant a loan and add to balance."""
        self.__loans[account] = self.__loans.get(account, 0) + amount
        account.set_balance(account.get_balance() + amount)
        print(f"💰 {account.get_name()} took a loan of {amount} at {interest_rate*100}% interest.")

    def kamat(self, account: BankAccount, rate=0.05):
        """Generator for interest calculation."""
        if account not in self.__loans:
            yield 0
        else:
            owed = self.__loans[account]
            while True:
                owed *= (1 + rate)
                yield owed

    def torleszt(self, account: BankAccount, payment: float):
        """Repay a loan."""
        if account not in self.__loans:
            print("✅ No active loan.")
            return
        self.__loans[account] -= payment
        if self.__loans[account] <= 0:
            del self.__loans[account]
            print(f"✅ {account.get_name()} fully repaid the loan.")
        else:
            print(f"💸 {account.get_name()} still owes {self.__loans[account]:.2f}")


# Feladat:
# egy privát szótárban legyenek eltárolva a felhasználók ahol a kulcs a BankAccount példány
# a value pedig egy felhasználónév + jelszó hash rendezett páros
# transaction method, a helyes bankszámlaszám jelszó párossal lehet pénzt felvenni valamint pénzt utalni
# close account method, törli egy felhasználó adatát a szótárból
# hitel: egy felhasználó kérhet hitelt, ebben az esetben eltárolásra kerül egy külön hitelezettek objektumban a tartozott összeggel
# valamint a felvett összeg rákerül a számlájára. A hitel a kamat() metódussal kamatozik (generátor) és a törleszt() metódussal törleszthető