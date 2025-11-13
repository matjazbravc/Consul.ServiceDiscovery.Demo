---
mode: agent
description: "Code Review Checklist for detester Project"
---

## Code Quality and Readability

- [ ] Is the code well-structured and easy to understand?
- [ ] Are variable, function, and class names self-explanatory?
- [ ] Is there any redundant or dead code?
- [ ] Are there any areas where code could be simplified?

## Documentation and Comments

- [ ] Are complex procedures, classes, or functions properly documented?
- [ ] Is there sufficient inline commenting where necessary?
- [ ] Are README and doc files accurate and up-to-date?
- [ ] Are public APIs and methods well documented?

## Functionality and Correctness

- [ ] Does the code perform the intended function or solve the stated problem?
- [ ] Are all edge cases handled properly?
- [ ] Is error handling robust and informative?
- [ ] Are test cases present and comprehensive (unit/integration)?

## Consistency and Style

- [ ] Is the code consistent with the project's style guides (e.g., indentation, spacing, brace placement)?
- [ ] Are there any formatting or linting issues?
- [ ] Are third-party libraries used appropriately and wisely?

## Security and Performance

- [ ] Are there any obvious security concerns (e.g., input validation, data exposure)?
- [ ] Is the code optimized for performance where necessary?
- [ ] Are resource-intensive operations handled and justified?

## Testing and CI/CD

- [ ] Are automated tests present and passing?
- [ ] Is the code integrated with the project's CI/CD pipeline?
- [ ] Is code coverage adequate for new and changed components?

## Suggestions & Summary

- [ ] List any blocking issues or mandatory changes.
- [ ] Suggest improvements or refactors.
- [ ] Summarize overall impression and next steps.

---

## Code Quality Sign-Off

At the end of your review, please rate the overall code quality by marking ONE of the following:

- `🟢 Great` — Code is robust, clean, and meets all standards.
- `🟡 Good` — Code is solid but there are some minor improvements recommended.
- `🔴 Not Good` — Code has significant issues that should be addressed.
