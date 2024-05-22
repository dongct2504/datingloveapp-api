import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'age'
})
export class AgePipe implements PipeTransform {

  transform(value: string): number {
    if (!value) {
      return 0;
    }

    const birthDate = new Date(value);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();

    if (today.getDate() < birthDate.getDate()) {
      age--;
    }

    return age;
  }
}