# VimWord

A Vim emulator add-in for Microsoft Word that brings powerful Vim-style keyboard navigation and editing modes to your Word documents.

## Features

### ?? Vim Modes

- **Normal Mode** - Navigate and execute commands with Vim keybindings
- **Insert Mode** - Standard Word text editing
- **Visual Mode** - Select text using Vim motions with anchor-based selection

### ?? Motion Support

#### Character Motions
- `h` - Move left (does not wrap lines)
- `l` - Move right (does not wrap lines)
- `j` - Move down one line
- `k` - Move up one line

#### Word Motions
- `w` - Move forward to start of next word
- `W` - Move forward to start of next WORD (includes punctuation)
- `b` - Move backward to start of previous word
- `B` - Move backward to start of previous WORD
- `e` - Move to end of current/next word
- `E` - Move to end of current/next WORD

#### Line Motions
- `0` - Move to start of line
- `$` - Move to end of line
- `_` - Move to first non-blank character of line

### ?? Visual Mode Features

- **Anchor-based selection** - Selection always includes the position where Visual mode started
- **Bidirectional selection** - Extend and shrink selection in both directions seamlessly
- **Smart motion handling** - Motions automatically extend or shrink selection based on direction
- **Automatic direction switching** - Active end switches when crossing the anchor point

#### Visual Mode Behaviors
- Extending with `e`, `w`, or `$` includes the target character
- Shrinking with `b` excludes trailing whitespace
- Pressing opposite motion (e.g., `b` after `e`) shrinks selection intelligently
- Selection collapses to anchor when fully shrunk, ready to extend in opposite direction

### ?? Mode Transitions

- `i` - Enter Insert mode at cursor
- `a` - Enter Insert mode after cursor
- `v` - Enter Visual mode (character-wise)
- `Esc` - Return to Normal mode from any mode

## Technical Highlights

### Architecture

- **KeyChord System** - Type-safe key combination handling with modifier support
- **Motion Interface** - Unified motion system with directional metadata
- **Anchor-based Visual Mode** - Proper Vim semantics for bidirectional selection
- **No Code Duplication** - Shared utilities and unified word/WORD motion implementations

### Motion Properties

Each motion declares:
- `Direction` - Forward, Backward, or Neutral
- `IncludesTarget` - Whether Visual mode should select through the target character

### Word vs WORD

- **word** - Alphanumeric + underscore; punctuation is separate
  - Example: `hello.world` has 3 words: "hello", ".", "world"
- **WORD** - Any non-whitespace characters
  - Example: `hello.world` is 1 WORD: "hello.world"

## Requirements

- Microsoft Word 2016 or later
- .NET Framework 4.8
- Windows

## Installation

Coming soon...

## Development

### Building

```bash
# Open in Visual Studio
# Build solution (F6)
```

### Project Structure

```
vimword/
??? AddIn/              # VSTO add-in infrastructure
??? UI/                 # Ribbon and user controls
??? Vimulator/          # Core Vim emulation engine
?   ??? Modes/         # Normal, Insert, Visual modes
?   ??? Motions/       # Motion implementations
?   ??? KeyChord.cs    # Keyboard input handling
??? docs/              # Documentation
```

### Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.

## Roadmap

- [ ] Operators (`d`, `c`, `y`)
- [ ] Text objects (`iw`, `aw`, `i"`, etc.)
- [ ] Visual Line mode
- [ ] Search (`/`, `?`, `n`, `N`)
- [ ] Marks
- [ ] Registers
- [ ] `.` (repeat) command
- [ ] Macros
- [ ] Configuration file

## License

Coming soon...

## Credits

Built with ?? for Vim enthusiasts who need to use Microsoft Word.

## Links

- [GitHub Repository](https://github.com/phishbacon/vimword)
- [Report Issues](https://github.com/phishbacon/vimword/issues)
