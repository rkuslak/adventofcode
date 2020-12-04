import re

INPUT_FILE = "2020-02-input.txt"
PASSWORD_LINE_REGEX = re.compile(r"([0-9]*)-([0-9]*)\s*([a-zA-Z]):\s*(.*)$")


def read_file(filename):
    with open(filename, "r") as infile:
        lines = infile.readlines()

    return lines


def new_validate_password_line(password_line):
    matches = PASSWORD_LINE_REGEX.match(password_line)
    assert matches is not None
    lower_bound_str, upper_bound_str, required_char, password = matches.groups()
    low_bound = int(lower_bound_str)
    upper_bound = int(upper_bound_str)

    first_char = password[low_bound - 1]
    second_char = password[upper_bound - 1]

    if first_char == second_char or required_char not in [first_char, second_char]:
        return False
    return True


def validate_password_line(password_line):
    matches = PASSWORD_LINE_REGEX.match(password_line)
    assert matches is not None
    lower_bound_str, upper_bound_str, required_char, password = matches.groups()
    low_bound = int(lower_bound_str)
    upper_bound = int(upper_bound_str)

    first_char = password[low_bound - 1]
    second_char = password[upper_bound - 1]

    char_count = 0
    for idx in range(len(password)):
        if password[idx] == required_char:
            char_count += 1
            if char_count > upper_bound:
                return False

    return low_bound <= char_count


def count_valid_passwords():
    password_lines = read_file(INPUT_FILE)
    valid_passwords = 0

    for password_line in password_lines:
        if new_validate_password_line(password_line):
            valid_passwords += 1

    return valid_passwords


if __name__ == "__main__":
    valid_passwords = count_valid_passwords()
    print(f"{valid_passwords} valid passwords")