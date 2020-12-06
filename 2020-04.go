package main

import (
	"fmt"
	"strconv"
	"strings"
)

func validateYearString(yearString string, min int, max int) bool {
	if len(yearString) == 4 {
		year, err := strconv.Atoi(yearString)
		if err == nil && year <= max && year >= min {
			return true
		}
	}

	return false
}

func validateHeightString(heightString string) bool {
	heightStringSize := len(heightString)
	if len(heightString) < 3 {
		return false
	}

	heightType := heightString[heightStringSize-2 : heightStringSize]
	heightString = heightString[:heightStringSize-2]
	height, err := strconv.Atoi(heightString)
	if err != nil {
		return false
	}
	switch heightType {
	case "cm":
		return height >= 150 && height <= 193
	case "in":
		return height >= 59 && height <= 76
	default:
		return false
	}
}

func validatePassport(passport_lines []string, ignore_cid bool) bool {
	has_byr := false
	has_iyr := false
	has_eyr := false
	has_hgt := false
	has_hcl := false
	has_ecl := false
	has_pid := false
	has_cid := false

	for _, line := range passport_lines {
		tokens := strings.Split(line, " ")
		for _, token := range tokens {
			s := strings.Split(token, ":")
			if len(s) < 2 {
				continue
			}

			field := s[0]
			value := s[1]

			switch field {
			case "byr":
				has_byr = validateYearString(value, 1920, 2002)
			case "iyr":
				has_iyr = validateYearString(value, 2010, 2020)
			case "eyr":
				has_eyr = validateYearString(value, 2020, 2030)
			case "hgt":
				has_hgt = validateHeightString(value)
			case "hcl":
				if len(value) > 2 && value[0] == '#' {
					value = value[1:]
					_, err := strconv.ParseInt(value, 16, 64)
					has_hcl = err == nil
				}
			case "ecl":
				switch value {
				case "amb":
					has_ecl = true
				case "blu":
					has_ecl = true
				case "brn":
					has_ecl = true
				case "gry":
					has_ecl = true
				case "grn":
					has_ecl = true
				case "hzl":
					has_ecl = true
				case "oth":
					has_ecl = true
				default:
					has_ecl = false
				}
			case "pid":
				if len(value) == 9 {
					_, err := strconv.Atoi(value)
					has_pid = err == nil
				}
			case "cid":
				has_cid = true
			default:
				fmt.Printf("Invalid field: %s", field)
			}
		}
	}

	return has_byr && has_iyr && has_eyr && has_hgt && has_hcl && has_ecl && has_pid && (has_cid || ignore_cid)
}

func run202004() {
	lines, err := LoadFile("2020-04-input.txt")
	if err != nil {
		panic(err)
	}

	validPassports := 0
	foundPassports := 0
	passportLines := []string{}
	readLinesCount := len(lines)

	for passportLinesIdx := 0; passportLinesIdx < readLinesCount; {
		for passportLinesIdx < readLinesCount && lines[passportLinesIdx] != "" {
			passportLines = append(passportLines, lines[passportLinesIdx])
			passportLinesIdx++
		}

		passportLinesIdx++
		foundPassports++
		if validatePassport(passportLines, true) {
			validPassports++
		}
		passportLines = []string{}
	}

	fmt.Printf("Found: %d, valid: %d", foundPassports, validPassports)
}
