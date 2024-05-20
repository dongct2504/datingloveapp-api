export interface LocalUserDto {
    localUserId: string;
    firstName: string | null;
    lastName: string | null;
    userName: string;
    age: number;
    gender: string;
    knownAs: string | null;
    profilePictureUrl: string | null;
    lastActive: string;
    address: string | null;
    ward: string | null;
    district: string | null;
    city: string | null;
}