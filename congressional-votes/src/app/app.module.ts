import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BaseUrlInterceptor } from './base-url-interceptor';
import { SenatorDetailComponent } from './senator-detail/senator-detail.component';
import { HomeComponent } from './home/home.component';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { AppNavBarComponent } from './app-nav-bar/app-nav-bar.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';

@NgModule({
  declarations: [
    AppComponent,
    SenatorDetailComponent,
    HomeComponent,
    AppNavBarComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatInputModule,
    MatSelectModule,
    MatCardModule,
    FlexLayoutModule,
    NgxSkeletonLoaderModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BaseUrlInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
