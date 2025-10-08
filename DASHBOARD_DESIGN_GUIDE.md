# Student Dashboard - Design System Guide

## 🎨 Color Palette

### Primary Colors
```
Blue (Trust & Primary Actions)
- Primary:   #4A90E2
- Accent:    #5DADE2
- Light:     #EBF5FB
- Use: Main actions, primary cards, links

Green (Success & Completion)
- Primary:   #27AE60
- Accent:    #58D68D
- Light:     #E8F8F5
- Use: Today's exams, completed status, success messages

Orange (Attention & Upcoming)
- Primary:   #F39C12
- Accent:    #F8B739
- Light:     #FEF5E7
- Use: Upcoming items, warnings, pending actions
```

### Neutral Colors
```
Text Colors:
- Dark:      #2C3E50 (Primary text)
- Light:     #7F8C8D (Secondary text)
- White:     #FFFFFF (Backgrounds, light text)

Backgrounds:
- Main BG:   #F8F9FA (Page background)
- Card BG:   #FFFFFF (Card backgrounds)
- Border:    #E8E8E8 (Dividers, borders)
```

## 📐 Layout Structure

```
┌─────────────────────────────────────────────────────────┐
│  Profile Header (Gradient Blue Background)              │
│  ┌─────┐  Welcome, [Student Name]!                     │
│  │Photo│  [Class Information]                           │
│  └─────┘                                                │
└─────────────────────────────────────────────────────────┘
         ↓
┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐
│ Total    │ │ Today's  │ │ Upcoming │ │ Average  │
│ Exams    │ │ Exams    │ │ Exams    │ │ Score    │
│ [BLUE]   │ │ [GREEN]  │ │ [ORANGE] │ │ [BLUE]   │
└──────────┘ └──────────┘ └──────────┘ └──────────┘
         ↓
┌─────────────────────────────────────────────────────────┐
│ TODAY'S EXAMS                                 [2 Exams] │
├─────────────────────────────────────────────────────────┤
│ ┌─────────────────────────────────────┐                │
│ │ Mathematics Quiz                    │ [Start Exam]   │
│ │ 🕐 09:00-10:30 | ⏱ 90 mins         │                │
│ └─────────────────────────────────────┘                │
└─────────────────────────────────────────────────────────┘
         ↓
┌─────────────────────────────────────────────────────────┐
│ UPCOMING EXAMS                                [5 Exams] │
├─────────────────────────────────────────────────────────┤
│ ┌─────────────────────────────────────┐                │
│ │ Physics Test                        │ [View Details] │
│ │ 📅 Dec 15 | 🕐 10:00-11:30         │                │
│ └─────────────────────────────────────┘                │
└─────────────────────────────────────────────────────────┘
         ↓
┌─────────────────────────────────────────────────────────┐
│ PERFORMANCE OVERVIEW                                    │
├─────────────────────────────────────────────────────────┤
│ Average Score    ████████░░ 85.0%                      │
│ Completion Rate  ██████████ 92.5%                      │
│ Tests Completed  █████░░░░░ 12 Tests                   │
└─────────────────────────────────────────────────────────┘
```

## 🎯 Component Specifications

### Stat Cards
```
Dimensions:
- Min Width: 280px
- Padding: 24px
- Border Radius: 12px
- Shadow: 0 4px 6px rgba(0,0,0,0.1)

Elements:
- Colored left border (4px)
- Icon container (56x56px, rounded 12px)
- Title (14px, uppercase, gray)
- Value (32px, bold, dark)
- Subtitle (14px, gray)

Hover Effect:
- Translate Y: -4px
- Shadow: 0 8px 15px rgba(0,0,0,0.15)
- Transition: 0.3s ease
```

### Profile Header
```
Background: Linear gradient (135deg)
- Start: #4A90E2
- End: #5DADE2

Avatar:
- Size: 100x100px
- Border: 4px white
- Border Radius: 50%
- Shadow: 0 4px 8px rgba(0,0,0,0.2)
- Fallback: Initial letter, centered

Text:
- Name: 28px, bold, white
- Info: 16px, white with 95% opacity
```

### Exam Cards
```
Layout:
- Padding: 20px
- Border: 1px solid #E8E8E8
- Border Radius: 10px
- Display: Flex (space-between)

Hover State:
- Border Color: #4A90E2
- Background: #EBF5FB
- Transform: translateX(4px)

Action Button:
- Padding: 10px 20px
- Border Radius: 8px
- Background: #4A90E2
- Color: White
- Hover Scale: 1.05
```

### Performance Bars
```
Container:
- Height: 32px
- Background: #F0F0F0
- Border Radius: 16px

Bar:
- Height: 100%
- Gradient: Linear 90deg
- Border Radius: 16px
- Animation: Width 1s ease
- Text: White, 13px, bold

Colors:
- Blue: #4A90E2 → #5DADE2
- Green: #27AE60 → #58D68D
- Orange: #F39C12 → #F8B739
```

## 📱 Responsive Breakpoints

### Desktop (> 768px)
```
- Stats Grid: 4 columns (auto-fit)
- Sidebar: Visible, fixed
- Exam Cards: Horizontal layout
- Performance: Horizontal bars
- Font Sizes: Base (16px)
```

### Tablet (768px)
```
- Stats Grid: 2 columns
- Sidebar: Collapsible
- Exam Cards: Horizontal layout
- Performance: Horizontal bars
- Font Sizes: Base (16px)
```

### Mobile (< 768px)
```
- Stats Grid: 1 column (stacked)
- Sidebar: Overlay menu
- Exam Cards: Vertical stacking
- Action Buttons: Full width
- Performance: Vertical layout
- Font Sizes: Slightly reduced
```

## 🎭 Icons Used (Phosphor Icons)

### Navigation
```
- Dashboard:  ph-house
- Exams:      ph-exam
- Profile:    ph-user-circle
- Logout:     ph-sign-out
```

### Dashboard
```
- Total Exams:     ph-exam
- Today's Exams:   ph-calendar-check
- Upcoming:        ph-calendar-plus
- Average Score:   ph-chart-line-up
- Time:           ph-clock
- Duration:       ph-timer
- Date:           ph-calendar
- Start Exam:     ph-arrow-right
- View Results:   ph-check-circle
- View Details:   ph-eye
- Charts:         ph-chart-bar
- Empty State:    ph-calendar-x
```

## ✨ Animation Specifications

### Hover Transitions
```css
transition: all 0.3s ease;
```

### Card Hover
```css
transform: translateY(-4px);  /* Lift effect */
box-shadow: 0 8px 15px rgba(0,0,0,0.15);
```

### Button Hover
```css
transform: scale(1.05);       /* Slight grow */
background: [lighter shade];
```

### Progress Bar Animation
```css
/* Initial state */
width: 0%;

/* Animated state (100ms delay) */
width: [actual percentage]%;
transition: width 1s ease;
```

## 📏 Spacing System

### Padding/Margins
```
- Extra Small:  4px   (xs)
- Small:        8px   (sm)
- Medium:       16px  (md)
- Large:        24px  (lg)
- Extra Large:  32px  (xl)
- XXL:          48px  (xxl)
```

### Component Spacing
```
- Card Padding:         24-28px
- Section Gap:          24-32px
- Grid Gap:             24px
- Exam Card Gap:        16px
- Icon Gap:             6-12px
```

## 🔤 Typography

### Font Families
```
Primary: System UI fonts
- -apple-system
- BlinkMacSystemFont
- "Segoe UI"
- Roboto
- "Helvetica Neue"
```

### Font Sizes
```
- H2 (Profile):      28px / bold
- H3 (Stat Title):   14px / medium / uppercase
- H4 (Section):      20px / semibold
- Stat Value:        32px / bold
- Exam Name:         16px / semibold
- Exam Details:      14px / normal
- Button Text:       14px / medium
- Body Text:         16px / normal
- Small Text:        13-14px / normal
```

### Font Weights
```
- Normal:     400
- Medium:     500
- Semibold:   600
- Bold:       700
```

## 🎨 Shadow System

### Elevation Levels
```css
/* Level 1 - Cards, default */
box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);

/* Level 2 - Hover, elevated */
box-shadow: 0 8px 15px rgba(0, 0, 0, 0.15);

/* Level 3 - Profile avatar */
box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
```

## 🔘 Border Radius

### Consistent Rounding
```
- Small:      8px  (buttons, badges)
- Medium:     10px (exam cards)
- Large:      12px (stat cards, sections)
- Extra:      16px (profile header, progress bars)
- Circle:     50%  (avatar)
```

## 🎯 Interactive States

### Default → Hover → Active

#### Stat Cards
```
Default:  shadow-sm, position: 0
Hover:    shadow-lg, translateY(-4px)
Active:   (same as hover)
```

#### Exam Cards
```
Default:  border-gray, bg-white
Hover:    border-blue, bg-light-blue, translateX(4px)
Active:   (same as hover)
```

#### Buttons
```
Default:  bg-primary, scale(1)
Hover:    bg-accent, scale(1.05)
Active:   bg-darker, scale(0.98)
```

## 📊 Data Display Patterns

### Empty States
```
Structure:
┌─────────────────────┐
│                     │
│      [Icon]         │  64px, 30% opacity
│                     │
│  [Message Text]     │  16px, gray
│                     │
└─────────────────────┘

Padding: 40px all sides
Text Align: Center
```

### Badge System
```
.badge-today:
- Background: Light Orange (#FEF5E7)
- Text: Dark Orange (#F39C12)
- Padding: 4px 12px
- Radius: 12px
- Font: 12px, bold, uppercase

.badge-upcoming:
- Background: Light Blue (#EBF5FB)
- Text: Dark Blue (#4A90E2)
- (same dimensions as above)
```

## 🎓 Accessibility Notes

### Color Contrast
- All text meets WCAG AA standards
- Minimum 4.5:1 ratio for normal text
- Minimum 3:1 ratio for large text

### Interactive Elements
- Minimum 44x44px touch target
- Clear focus indicators
- Semantic HTML5 elements
- Alt text for images

### Responsive Text
- Base: 16px (100% readable)
- Scales appropriately on mobile
- Line height: 1.5 for body text

## 🔧 CSS Variables Usage

```css
:root {
  /* Colors */
  --primary-blue: #4A90E2;
  --primary-green: #27AE60;
  --primary-orange: #F39C12;
  
  /* Shadows */
  --shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  --shadow-hover: 0 8px 15px rgba(0, 0, 0, 0.15);
}

/* Usage */
.stat-card {
  box-shadow: var(--shadow);
}

.stat-card.blue .stat-card-icon {
  color: var(--primary-blue);
}
```

## 📱 Mobile Optimization

### Touch Targets
```
Minimum size: 44x44px
- Buttons: Full width or large enough
- Links: Adequate padding
- Cards: Entire card tappable
```

### Layout Adjustments
```
- Remove hover effects
- Increase spacing
- Stack elements vertically
- Full-width buttons
- Larger text for readability
```

## 🎉 Conclusion

This design system ensures consistency, accessibility, and maintainability across the Student Dashboard. All components follow a unified visual language with the academic color scheme (blue, green, orange) and modern UI principles.

---

**Design Philosophy**: Clean, Modern, Academic, Action-Oriented  
**Inspiration**: Material Design, Bootstrap, Modern SaaS Dashboards  
**Accessibility**: WCAG 2.1 AA Compliant



