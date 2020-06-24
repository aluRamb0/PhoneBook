import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPhoneBook } from 'src/types/phonebook.interface';
import { Observable, throwError } from 'rxjs'
import { catchError, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn:'root'
})
export class PhoneBookService
{
    baseUrl: string = environment.baseUrl;
    constructor(private http: HttpClient) { }

    getHeaders() :HttpHeaders{
        var headers = new HttpHeaders({
            "Content-Type": "application/json",
            "Accept": "*/*"
        });
        return headers;
    }
    getAll():Observable<Array<IPhoneBook>>{
        var url = this.baseUrl;
        return this.http.get(url, { headers: this.getHeaders() })
        .pipe(
            tap((data:Array<IPhoneBook>) => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    get(id:any):Observable<IPhoneBook>{
        var url = this.baseUrl + "/" + id;
        return this.http.get(url, { headers: this.getHeaders() })
        .pipe(
            tap((data:IPhoneBook) => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
    }


    create(input: IPhoneBook):Observable<any>{
        var url = this.baseUrl;
        return this.http.post(url, input, { headers: this.getHeaders() })
        .pipe(
            tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    update(input: IPhoneBook):Observable<any>{
        var url = this.baseUrl + "/" + input.id;
        return this.http.put(url, input, { headers: this.getHeaders() })
        .pipe(
            tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
    }
    deletePhoneBook(id:any):Observable<any>{
        var url = this.baseUrl + "/" + id;
        return this.http.delete(url, { headers: this.getHeaders() })
        .pipe(
            tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
    }
    deleteEntry(id:any, entryId:any):Observable<any>{
        var url = this.baseUrl + "/" + id + "/entry/" + entryId;
        return this.http.delete(url, { headers: this.getHeaders() })
        .pipe(
            tap(data => console.log('All: ' + JSON.stringify(data))),
            catchError(this.handleError)
        );
    }





    private handleError(err: HttpErrorResponse) {
        // in a real world app, we may send the server to some remote logging infrastructure
        // instead of just logging it to the console
        let errorMessage = '';
        if (err.error instanceof ErrorEvent) {
            // A client-side or network error occurred. Handle it accordingly.
            errorMessage = `An error occurred: ${err.error.message}`;
        } else {
            // The backend returned an unsuccessful response code.
            // The response body may contain clues as to what went wrong,
            errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
        }
        console.error(errorMessage);
        return throwError(errorMessage);
    }
}

