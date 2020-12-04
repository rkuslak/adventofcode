INPUT_FILE = "2020-03-input.txt"


def read_file(filename):
    with open(filename, "r") as infile:
        lines = [str.strip(x) for x in infile.readlines()]

    return lines


def count_trees(
    vert_offset=3,
    horizontal_offset=1,
    tree_char="#",
):
    map_lines = read_file(INPUT_FILE)
    first_line = map_lines[0]
    line_size = len(first_line) - 1

    tree_count = 0
    line_idx = 0

    for map_idx in range(0, len(map_lines), horizontal_offset):
        while line_idx > line_size:
            line_idx -= line_size + 1

        map_line = map_lines[map_idx]
        if map_line[line_idx] == tree_char:
            tree_count += 1

        line_idx += vert_offset

    return tree_count


if __name__ == "__main__":
    s = 0
    offsets = [
        (1, 1),
        (3, 1),
        (5, 1),
        (7, 1),
        (1, 2),
    ]
    for vert_offset, h_offset in offsets:
        result = count_trees(vert_offset, h_offset)
        if s > 0:
            s *= result
        else:
            s = result

    print(s)