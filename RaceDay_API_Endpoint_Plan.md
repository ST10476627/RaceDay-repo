# RaceDay RESTful API Endpoint Specification

## Section B – API Endpoint Plan

| HTTP Method | Route | Description | Role Required | Request Body (if any) | Expected Response |
|---|---|---|---|---|---|
| POST | `/api/auth/register` | Registers a new user as an Organiser or Participant. | Public | `{ "firstName": "...", "lastName": "...", "email": "...", "password": "...", "roleId": 1 }` | `201 Created`, `400 Bad Request`, `409 Conflict` |
| POST | `/api/auth/login` | Authenticates a registered user and starts an authenticated session. | Public | `{ "email": "...", "password": "..." }` | `200 OK`, `400 Bad Request`, `401 Unauthorized` |
| POST | `/api/auth/logout` | Logs out the currently authenticated user. | Authenticated | None | `200 OK`, `401 Unauthorized` |
| GET | `/api/users/me` | Retrieves the authenticated user's profile. | Authenticated | None | `200 OK`, `401 Unauthorized`, `404 Not Found` |
| PUT | `/api/users/me` | Updates the authenticated user's profile details. | Authenticated | `{ "firstName": "...", "lastName": "...", "email": "..." }` | `200 OK`, `400 Bad Request`, `401 Unauthorized`, `409 Conflict` |
| GET | `/api/events` | Retrieves all available events. | Authenticated | None | `200 OK`, `401 Unauthorized` |
| GET | `/api/events/{id}` | Retrieves the details of a specific event. | Authenticated | None | `200 OK`, `401 Unauthorized`, `404 Not Found` |
| POST | `/api/events` | Creates a new event. | Organiser | `{ "eventName": "...", "eventDescription": "...", "eventDate": "...", "distance": 10.00, "eventType": "Run", "location": "..." }` | `201 Created`, `400 Bad Request`, `401 Unauthorized`, `403 Forbidden` |
| PUT | `/api/events/{id}` | Updates an event managed by the authenticated Organiser. | Organiser | `{ "eventName": "...", "eventDescription": "...", "eventDate": "...", "distance": 10.00, "eventType": "Run", "location": "..." }` | `200 OK`, `400 Bad Request`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found` |
| DELETE | `/api/events/{id}` | Deletes an event managed by the authenticated Organiser. | Organiser | None | `204 No Content`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found` |
| GET | `/api/events/{eventId}/categories` | Retrieves all categories for a specific event. | Authenticated | None | `200 OK`, `401 Unauthorized`, `404 Not Found` |
| POST | `/api/events/{eventId}/categories` | Creates an age or distance category for an event. | Organiser | `{ "categoryName": "...", "categoryType": "Age" }` | `201 Created`, `400 Bad Request`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found` |
| PUT | `/api/categories/{id}` | Updates an existing category. | Organiser | `{ "categoryName": "...", "categoryType": "Distance" }` | `200 OK`, `400 Bad Request`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found` |
| DELETE | `/api/categories/{id}` | Deletes an existing category. | Organiser | None | `204 No Content`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found` |
| POST | `/api/events/{eventId}/enrolments` | Enrols the authenticated Participant in an event using a selected category. | Participant | `{ "categoryId": 1 }` | `201 Created`, `400 Bad Request`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found`, `409 Conflict` |
| GET | `/api/enrolments/me` | Retrieves all enrolments belonging to the authenticated Participant. | Participant | None | `200 OK`, `401 Unauthorized`, `403 Forbidden` |
| GET | `/api/events/{eventId}/enrolments` | Retrieves all enrolments for an event managed by the authenticated Organiser. | Organiser | None | `200 OK`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found` |
| PUT | `/api/enrolments/{id}/status` | Updates an enrolment status, such as Pending or Confirmed. | Organiser | `{ "enrolmentStatus": "Confirmed" }` | `200 OK`, `400 Bad Request`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found` |
| POST | `/api/enrolments/{enrolmentId}/results` | Captures a finish time and finishing position for an enrolled Participant. | Organiser | `{ "finishTime": "04:45:30", "finishPosition": 102 }` | `201 Created`, `400 Bad Request`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found`, `409 Conflict` |
| PUT | `/api/results/{id}` | Updates an existing result. | Organiser | `{ "finishTime": "04:40:00", "finishPosition": 95 }` | `200 OK`, `400 Bad Request`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found` |
| GET | `/api/results/me` | Retrieves the authenticated Participant's results and performance history. | Participant | None | `200 OK`, `401 Unauthorized`, `403 Forbidden` |

## Notes

- `Authenticated` means either an Organiser or Participant must be logged in.
- Event creation, updating, deletion, category management, enrolment viewing, and result capture are restricted to Organisers.
- Event enrolment and viewing personal enrolments/results are restricted to Participants.
- The Participant ID does not need to be supplied in the enrolment request body because it can be obtained from the authenticated user session.
- The Event ID is supplied in the enrolment route, while the selected Category ID is supplied in the request body.
- The Result endpoint uses an Enrolment ID because the `Results` table is linked directly to `Enrolment` in the RaceDay database.
