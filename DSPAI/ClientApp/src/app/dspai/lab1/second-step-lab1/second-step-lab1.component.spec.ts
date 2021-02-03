import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SecondStepLab1Component } from './second-step-lab1.component';

describe('SecondStepLab1Component', () => {
  let component: SecondStepLab1Component;
  let fixture: ComponentFixture<SecondStepLab1Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SecondStepLab1Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SecondStepLab1Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
