# Exam Absent Status Feature

## Overview

When an exam has ended and the student did not attempt it, the system now shows an **"Absent"** status with a **"Time Out"** message, clearly indicating that the student was marked absent for that exam.

---

## Feature Description

### Previous Behavior
- **Ended Exam (Not Attempted)**: Showed "Exam Has Ended" with disabled button

### New Behavior
- **Ended Exam (Not Attempted)**: Shows **"⏱️ Time Out - Absent"** with clear absent status
- **Ended Exam (Attempted)**: Shows "Exam Completed" with "View Results" button

---

## Visual Representation

### State 1: Exam Completed (Attempted)

```
┌─────────────────────────────────────────────┐
│                                              │
│  ┌──────────────────────────────────────┐  │
│  │      ✅                                │  │
│  │  Exam Completed                       │  │ ← Green
│  └──────────────────────────────────────┘  │
│                                              │
│  ┌──────────────────────────────────────┐  │
│  │  👁️ View Results                     │  │ ← Enabled
│  └──────────────────────────────────────┘  │
│                                              │
└─────────────────────────────────────────────┘
```

### State 2: Time Out - Absent (Not Attempted)

```
┌─────────────────────────────────────────────┐
│                                              │
│  ┌──────────────────────────────────────┐  │
│  │      👤❌                              │  │
│  │  ⏱️ Time Out - Absent                 │  │ ← Orange/Red
│  │  You did not attempt this exam        │  │ ← Submessage
│  └──────────────────────────────────────┘  │
│                                              │
│  ┌──────────────────────────────────────┐  │
│  │  🚫 Marked as Absent                  │  │ ← Disabled
│  └──────────────────────────────────────┘  │
│                                              │
└─────────────────────────────────────────────┘
```

---

## Complete State Flow

```
┌──────────────────┐
│  NOT STARTED     │
│  (Future)        │
│  ⏰ Countdown    │
│  🔒 Disabled     │
└────────┬─────────┘
         │ Time reaches 0
         ↓
┌──────────────────┐
│     ACTIVE       │
│  (Current)       │
│  ✅ Pulse        │
│  🟢 Enabled      │
└────────┬─────────┘
         │ Duration expires
         ↓
    ┌────┴─────┐
    │          │
    ↓          ↓
┌─────────┐  ┌──────────┐
│COMPLETED│  │ ABSENT   │
│(Attempt)│  │(No Try)  │
│✅ Green │  │⏱️ Orange│
│View Res │  │🚫 Disabled│
└─────────┘  └──────────┘
```

---

## Detailed States

### State 1: Exam Completed ✅

**Condition:**
- Exam has ended
- Student attempted the exam (`isAttempted = true`)

**Display:**
- **Background**: Light red (#FADBD8)
- **Border**: Red (#E74C3C)
- **Icon**: Check circle (green)
- **Message**: "Exam Completed"
- **Button**: "View Results" (Enabled, Blue)

**Purpose:**
- Allow student to view their results
- Positive feedback for completion

---

### State 2: Time Out - Absent ⏱️

**Condition:**
- Exam has ended
- Student did NOT attempt the exam (`isAttempted = false`)

**Display:**
- **Background**: Light orange/beige (#FDF4E3)
- **Border**: Orange (#E67E22)
- **Icon**: User minus icon (👤❌)
- **Main Message**: "⏱️ Time Out - Absent"
- **Sub Message**: "You did not attempt this exam"
- **Button**: "Marked as Absent" (Disabled, Gray)

**Purpose:**
- Clear indication of absence
- Time out message shows exam expired
- No action available (can't take exam anymore)

---

## CSS Styling

### Absent State Colors

```css
.exam-state.absent {
    background: #FDF4E3;        /* Light orange/beige */
    border: 2px solid #E67E22;  /* Orange border */
}

.exam-state.absent .exam-state-icon {
    color: #E67E22;             /* Orange icon */
}

.exam-state.absent .exam-state-message {
    color: #E67E22;             /* Orange text */
}
```

### Sub-message Styling

```css
.exam-state-submessage {
    font-size: 14px;
    color: var(--text-light);   /* Gray text */
    margin: 8px 0 0 0;
}
```

---

## JavaScript Logic

### Decision Tree

```javascript
if (state === 'ended') {
    if (currentExamData.isAttempted === 'true') {
        // Show "Exam Completed" with "View Results" button
        showCompletedState();
    } else {
        // Show "Time Out - Absent" with disabled button
        showAbsentState();
    }
}
```

### Implementation

```javascript
// Student didn't attempt - show ABSENT status
stateSection.innerHTML = `
    <div class="exam-state absent">
        <div class="exam-state-icon">
            <i class="ph ph-user-minus"></i>
        </div>
        <p class="exam-state-message">⏱️ Time Out - Absent</p>
        <p class="exam-state-submessage">You did not attempt this exam</p>
    </div>
`;

actionSection.innerHTML = `
    <button class="exam-modal-action" disabled>
        <i class="ph ph-prohibit"></i>
        Marked as Absent
    </button>
`;
```

---

## Icon Usage

### Icons by State

| State | Icon | Code | Color |
|-------|------|------|-------|
| **Not Started** | ⏰ Clock | `ph-clock-countdown` | Orange |
| **Active** | ✅ Check | `ph-check-circle` | Green (Pulse) |
| **Completed** | ✅ Check | `ph-check-circle` | Red |
| **Absent** | 👤❌ User Minus | `ph-user-minus` | Orange |

### Button Icons

| Button | Icon | Code |
|--------|------|------|
| **Not Available** | 🔒 Lock | `ph-lock` |
| **Start Exam** | ▶️ Play | `ph-play-circle` |
| **View Results** | 👁️ Eye | `ph-eye` |
| **Marked Absent** | 🚫 Prohibit | `ph-prohibit` |

---

## User Experience

### Scenario 1: Student Attempts Exam

```
Timeline:
10:00 AM - Exam starts
10:30 AM - Student starts exam
11:45 AM - Student submits exam
12:00 PM - Exam ends

Result:
✅ Shows "Exam Completed"
🟢 "View Results" button enabled
```

### Scenario 2: Student Misses Exam

```
Timeline:
10:00 AM - Exam starts
10:30 AM - Student doesn't start
12:00 PM - Exam ends

Result:
⏱️ Shows "Time Out - Absent"
👤❌ User minus icon
🚫 "Marked as Absent" button (disabled)
📝 Message: "You did not attempt this exam"
```

### Scenario 3: Student Opens During Exam (But Doesn't Attempt)

```
Timeline:
10:00 AM - Exam starts
10:15 AM - Student opens modal (sees "Active")
10:15 AM - Student closes modal without starting
12:00 PM - Exam ends
12:05 PM - Student opens modal again

Result:
⏱️ Shows "Time Out - Absent"
🚫 Marked as absent (didn't click "Start Exam")
```

---

## Color Psychology

### Orange/Beige (#E67E22, #FDF4E3)
- **Meaning**: Warning, attention needed
- **Use Case**: Absent status
- **Emotion**: Urgent but not critical
- **Purpose**: Alert student of absence

### Red (#E74C3C)
- **Meaning**: Stop, ended, cannot proceed
- **Use Case**: Exam ended (attempted)
- **Emotion**: Definitive, final
- **Purpose**: Show completion

### Green (#27AE60)
- **Meaning**: Success, go ahead, active
- **Use Case**: Active exam, completed
- **Emotion**: Positive, encouraging
- **Purpose**: Enable action

---

## Testing Scenarios

### Test Case 1: Past Exam - Not Attempted

**Setup:**
1. Create exam that ended yesterday
2. Ensure student didn't attempt it
3. Click exam card

**Expected Result:**
- Modal opens
- No countdown shown
- Orange/beige background
- Icon: User minus (👤❌)
- Message: "⏱️ Time Out - Absent"
- Submessage: "You did not attempt this exam"
- Button: "Marked as Absent" (disabled)

### Test Case 2: Past Exam - Attempted

**Setup:**
1. Create exam that ended yesterday
2. Ensure student attempted it
3. Click exam card

**Expected Result:**
- Modal opens
- No countdown shown
- Light red background
- Icon: Check circle (✅)
- Message: "Exam Completed"
- Button: "View Results" (enabled, clickable)

### Test Case 3: Exam Just Ended - Not Started

**Setup:**
1. Create exam ending in 1 minute
2. Open modal and watch countdown
3. Don't click "Start Exam"
4. Wait for countdown to reach zero
5. Wait for duration to expire

**Expected Result:**
- State changes from "Active" to "Absent"
- Shows "Time Out - Absent"
- Button becomes "Marked as Absent" (disabled)

---

## Benefits

### For Students
1. **Clear Feedback**: Knows they were marked absent
2. **Time Awareness**: "Time Out" indicates missed deadline
3. **No Confusion**: Can't accidentally try to start ended exam
4. **Visual Distinction**: Different color from completion

### For Teachers/Admin
1. **Accurate Tracking**: Clear absent vs completed status
2. **Better Analytics**: Can track absence rates
3. **Fair Assessment**: Students can't claim confusion

### For System
1. **Consistent State**: Clear logic for all scenarios
2. **User-Friendly**: Intuitive status indicators
3. **Professional**: Matches real exam systems

---

## Comparison with Other States

| Aspect | Not Started | Active | Completed | Absent |
|--------|-------------|--------|-----------|--------|
| **Timing** | Future | Current | Past (Tried) | Past (Missed) |
| **Color** | Orange | Green | Red | Orange |
| **Icon** | Clock | Check (Pulse) | Check | User Minus |
| **Countdown** | ✅ Visible | ❌ Hidden | ❌ Hidden | ❌ Hidden |
| **Button** | Disabled | Enabled | Enabled | Disabled |
| **Action** | Wait | Start Exam | View Results | None |
| **Message** | Not Started | Now Active | Completed | Time Out |

---

## Implementation Details

### Files Modified
- `Views/Dashboard/StudentDashboard.cshtml`

### Lines Changed
- **CSS**: Added `.exam-state.absent` styles
- **CSS**: Added `.exam-state-submessage` styles
- **JavaScript**: Updated `updateExamState('ended')` logic

### No Backend Changes
- All logic is client-side
- Uses existing `isAttempted` flag from exam data
- No database modifications needed

---

## Future Enhancements

### Potential Features
1. **Absence Reason**: Allow students to provide reason
2. **Email Notification**: Alert student when marked absent
3. **Absence Count**: Track total absences on dashboard
4. **Teacher View**: Show absence list for teachers
5. **Late Submission**: Allow grace period after time out
6. **Excuse Submission**: Upload medical certificate, etc.

### Analytics
1. Absence rate by student
2. Absence patterns (time of day, day of week)
3. Correlation with performance
4. Comparison across classes

---

## Troubleshooting

### Issue: Shows "Absent" for Attempted Exam
- **Cause**: `isAttempted` flag not set correctly
- **Solution**: Check backend logic for setting attempt status

### Issue: Shows "Completed" for Not Attempted
- **Cause**: `isAttempted` returning wrong value
- **Solution**: Verify data passed to modal

### Issue: Wrong Color Displayed
- **Cause**: CSS class conflict
- **Solution**: Check browser console, verify CSS loaded

---

## Conclusion

The "Time Out - Absent" status provides clear, professional feedback when students miss exams. The orange color scheme, user-minus icon, and explicit messaging ensure students understand their absence was recorded, while the disabled button prevents confusion about whether they can still take the exam.

This feature enhances the system's professionalism and matches real-world examination systems where absence tracking is crucial for academic records.

---

**Status**: ✅ Implemented
**Version**: 1.0
**Date**: October 2025


