export default interface AuthenticationResponse {
    authenticated: boolean;
    firstName:     string;
    lastName:      string;
    username:      string;
    token:         string;
}