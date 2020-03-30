import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from './../environments/environment';
import { Injectable } from '@angular/core';

@Injectable()
export class BaseUrlInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let requestUrl = req.url;
    if (!requestUrl.startsWith('http')) {
      let environmentUrl = environment.apiUrl;
      if (!environmentUrl.endsWith('/')) {
        environmentUrl += '/';
      }

      requestUrl = environmentUrl + requestUrl;
    }

    return next.handle(req.clone({ url: requestUrl }));
  }
}
