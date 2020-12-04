INPUT_FILE = "2020-01-input.txt"

def read_file(filename):
    with open(filename, "r") as infile:
        lines = infile.readlines()

    return lines

def find_join():
    expenses = sorted([int(str.strip(x)) for x in read_file(INPUT_FILE)])
    number_expenses = len(expenses)

    for left_idx in range(number_expenses):
        left = expenses[left_idx]
        for center_idx in range(left_idx + 1, number_expenses):
            center = expenses[center_idx]
            for right_idx in range(center_idx + 1, number_expenses):
                right = expenses[right_idx]
                if left + center + right == 2020:
                    return (left, center, right)



if __name__ == "__main__":
    result = find_join()
    if result is not None:
        left, center, right = result

        print("%d %d %d", left, right, left * right * center)