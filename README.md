# HONN-Verkefni-2

[![Build Status](https://travis-ci.org/hjalmar15/HONN-Verkefni-2.svg?branch=master)](https://travis-ci.org/hjalmar15/HONN-Verkefni-2)


| URI                                | HTTP Method | Service        | Útskýring                                          | Example Method call                                                                                                                                                                                                   | Example JSON body                                                                                                                                |
|------------------------------------|-------------|----------------|----------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------|
| /books                             | GET         | Books          | Sækja allar bækur                                  | http://localhost:5000/api/v1/books,,http://localhost:5000/api/v1/books?LoanDuration=30, http://localhost:5000/api/v1/books?LoanDate=2017-01-01, http://localhost:5000/api/v1/booksLoanDuration=30&LoanDate=2017-01-01 | {         "title": "New   book",         "author": "Some   Author",         "datePublished":   "2010-10-10",         "isbn":   "123456789"     } |
| /books                             | POST        | Books          | Bæta við bók                                       |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /books/{book_id}                   | GET         | Books          | Sækja öll gögn um bók (m.a. lánasögu)              |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /books/{book_id}                   | DELETE      | Books          | Fjarlæga bók                                       |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /books/{book_id}                   | PUT         | Books          | Uppfæra bók                                        |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users                             | GET         | Users          | Sækja alla notendur                                |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users                             | POST        | Users          | Bæta við notanda                                   |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}                   | GET         | Users          | Sækja upplýsingar um notanda (m.a. lánasögu)       |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}                   | DELETE      | Users          | Fjarlæga notanda                                   |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}                   | PUT         | Users          | Uppfæra notanda                                    |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}/books             | GET         | Books          | Sækja skráningu um bækur sem notandi er með í láni |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}/books/{book_id}   | POST        | Books          | Skrá útlán á bók                                   |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}/books/{book_id}   | DELETE      | Books          | Skila bók                                          |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}/books/{book_id}   | PUT         | Books          | Uppfæra útlánaskráningu                            |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}/reviews           | GET         | Reviews        | Sækja dóma fyrir notanda                           |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}/reviews/{book_id} | GET         | Reviews        | Sækja dóma fyrir bók                               |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}/reviews/{book_id} | POST        | Reviews        | Skrá dóma fyrir bók                                |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}/reviews/{book_id} | DELETE      | Reviews        | Fjarlæga dóma                                      |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}/reviews/{book_id} | PUT         | Reviews        | Uppfæra dóma um bók                                |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /users/{user_id}/recommendation    | GET         | Recommendation | Fá lista yfir vænlegar og ólesnar bækur            |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /books/reviews                     | GET         | Review         | Fá alla dóma fyrir allar bækur                     |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /books/{book_id}/reviews           | GET         | Reviews        | Fá alla dóma fyrir bók                             |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /books/{book_id}/reviews/{user_id} | GET         | Reviews        | Fá dóm frá notanda fyrir bók                       |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /books/{book_id}/reviews/{user_id} | PUT         | Reviews        | Breyta dómi notanda um bók                         |                                                                                                                                                                                                                       |                                                                                                                                                  |
| /books/{book_id}/reviews/{user_id} | DELETE      | Reviews        | Eyða dómi notanda um bók                           |                                                                                                                                                                                                                       |                                                                                                                                                  |