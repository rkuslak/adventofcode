package main

// RuneSet is a struct holding backing data for a given set of runes.
type RuneSet struct {
	runes   []rune
	lastIdx int
	maxSize int
}

// NewRuneSet creates a new "RuneSet" object, ensuring backing data is properly
// initialized
func NewRuneSet() *RuneSet {
	maxSize := 5
	runes := make([]rune, maxSize, maxSize)

	return &RuneSet{
		runes,
		0,
		maxSize,
	}
}

func (rs *RuneSet) ensureSpace() {
	// We are potentially adding a new value; ensure we have at least 1 remaining
	// address in the array open
	for rs.maxSize-1 <= rs.lastIdx {
		// If someone created the RuneSet struct manually, maxSize will be 0:
		if rs.maxSize < 1 {
			rs.maxSize = 2
		}
		rs.maxSize *= 2

		newRunes := make([]rune, rs.maxSize, rs.maxSize)
		for idx, r := range rs.runes {
			newRunes[idx] = r
		}
		rs.runes = newRunes
	}
}

// Add - Adds the given rune to the set if it does not exist.
func (rs *RuneSet) Add(newRune rune) {

	insertIdx := rs.lastIdx
	for ; ; insertIdx-- {
		if rs.runes[insertIdx] == newRune {
			return
		}

		// We want to break when the index is at 0 (so EVERYTHING needs to move) or
		// when it is at the index we need to insert at. This would be cleaner if we
		// KNEW we had a set coming in to this; maybe create has table and just
		// insert sort that would be faster?
		if insertIdx == 0 || rs.runes[insertIdx-1] < newRune {
			break
		}
	}

	if insertIdx < 0 {
		insertIdx = 0
	}

	// We now know we will be adding the rune; ensure we have space to actually
	// store it
	rs.ensureSpace()

	// Move runes if not adding as the last position in the array
	for moveIdx := rs.lastIdx; moveIdx > 0 && moveIdx > insertIdx; moveIdx-- {
		rs.runes[moveIdx] = rs.runes[moveIdx-1]
	}

	rs.lastIdx++
	rs.runes[insertIdx] = newRune
}

// GetRunes returns the dedupped "set" of runes containing all values supplied
func (rs *RuneSet) GetRunes() []rune {
	return rs.runes
}
