export interface RegisterLocalUserDto {
    userName: string;
    nickname: string;
    email: string;
    phoneNumber: string;
    dateOfBirth: string;
    gender: string;
    password: string;
    role: string | null;
}