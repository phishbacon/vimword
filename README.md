# VimWord

A Vim emulator add-in for Microsoft Word.
## Features

### 🎯 Vim Modes

- **Normal Mode** - Navigate and execute commands with Vim keybindings
- **Insert Mode** - Standard Word text editing
- **Visual Mode** - Select text using Vim motions with anchor-based selection

### ⌨️ Motion Support

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

### 🔧 Mode Transitions

- `i` - Enter Insert mode at cursor
- `a` - Enter Insert mode after cursor
- `v` - Enter Visual mode (character-wise)
- `Esc` - Return to Normal mode from any mode

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
├── AddIn/              # VSTO add-in infrastructure
├── UI/                 # Ribbon and user controls
├── Vimulator/          # Core Vim emulation engine
│   ├── Modes/         # Normal, Insert, Visual modes
│   ├── Motions/       # Motion implementations
│   └── KeyChord.cs    # Keyboard input handling
└── docs/              # Documentation
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

Built with ❤️ for Vim enthusiasts who need to use Microsoft Word.

## Links

- [GitHub Repository](https://github.com/phishbacon/vimword)
- [Report Issues](https://github.com/phishbacon/vimword/issues)
